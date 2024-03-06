using SmartRecipes.DataContext.Recipes.Generators.Models;
using SmartRecipes.DataContext.Recipes.Models;
using SmartRecipes.DataContext.Recipes.Generators.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.ObjectModel;

namespace SmartRecipes.DataContext.Recipes.Generators;

public sealed class DomainDataAdder : IDomainDataAdder
{
    private readonly RecipesContext db;
    public DomainDataAdder(RecipesContext db)
    {
        this.db = db;
    }

    public async Task<(bool, string?)> AddCategory(CreateCategoryModel model)
    {
        Category newEntity = new()
        {
            CategoryName = model.Name
        };
        await db.AddAsync(newEntity);
        int affected = await db.SaveChangesAsync();
        return affected == 1 ? (true, null) : (false, "Ошибка добавления");
    }

    public async Task<(bool, string?)> AddIngredient(CreateIngredientModel model)
    {
        Ingredient newEntity = new()
        {
            IngredientName = model.Name,
        };
        await db.AddAsync(newEntity);
        int affected = await db.SaveChangesAsync();
        return affected == 1 ? (true, null) : (false, "Ошибка добавления");
    }

    public async Task<(bool, string?)> AddRecipe(CreateRecipeModel model)
    {
        Category? foundCategory = await db.Categories.SingleOrDefaultAsync(e => e.CategoryName == model.CategoryName);
        if (foundCategory is null) { return (false, "Не найдена категория с таким именем"); }

        Image? foundImage = await db.Images.SingleOrDefaultAsync(e => e.ImageURL == model.ImageName);
        if (foundImage is not null) { return (false, $"Картинка уже используется для {foundImage.RecipeWhereUsed.RecipeName}"); }

        Image newImage = new()
        {
            ImageURL = Path.Combine("img", "recipes", model.ImageName)
        };
        var addedImage = await db.Images.AddAsync(newImage);

        Recipe newRecipe = new()
        {
            RecipeName = model.RecipeName,
            RecipeDescription = RecipeDescriptionHandler.ToHTML(model.RecipeDescription),
            Category = foundCategory,
            TimeToCook = model.TimeToCook,
            Ingredients = await db.Ingredients
                .Where(e => model.Ingredients
                    .Select(m => m.Key)
                    .Contains(e.IngredientName))
                    .ToListAsync(),
            RecipeImage = addedImage.Entity,
        };
        var addedRecipe = await db.Recipes.AddAsync(newRecipe);
        var recipe = addedRecipe.Entity;
        var img = addedImage.Entity;
        img.RecipeWhereUsed = recipe;
        addedImage.State = EntityState.Modified;

        var amounts = model.Ingredients
                .Select(x => new IngredientAmountForRecipe()
                {
                    IngredientID = db.Ingredients.FirstOrDefault(y => y.IngredientName == x.Key)?.ID ?? "0",
                    Ingredient = db.Ingredients.FirstOrDefault(y => y.IngredientName == x.Key) ?? new(),
                    RecipeID = addedRecipe.Entity.ID,
                    Recipe = addedRecipe.Entity,
                    Amount = x.Value
                });
        db.AddRange(amounts);

        recipe.IngredientsAmounts = amounts.ToList();
        recipe.Rating = await addRatesAsync("initialDataFill_AdminID", recipe, model.Rating["likes"], model.Rating["dislikes"]);
        foundCategory.RecipesWhereUsed.Add(newRecipe);
        addedRecipe.State = EntityState.Modified;
        
        int affected = await db.SaveChangesAsync();
        if (affected == 0) return (false, "Ошибка при обновлении базы данных");

        return (true, null);

    }
    private async Task<ICollection<Rate>> addRatesAsync(string adminId, Recipe recipe, int likesCount, int dislikesCount)
    {
        var newRates = Enumerable.Range(1, likesCount + dislikesCount)
            .Select(x => new Rate()
            {
                UserID = adminId,
                RecipeRated = recipe,
                RecipeRatedID = recipe.ID,
                RateType = x <= likesCount ? "like" : "dislike"
            })
            .ToList();
        db.Ratings.AddRange(newRates);
        return await db.Ratings.Where(x => x.UserID == adminId).ToListAsync();

    }

    public async Task<(bool, string?)> AddShop(CreateShopModel model)
    {
        var notExistingIngredients = db.Ingredients
             .ExceptBy(model.AvalableIngredients.Keys, x => x.IngredientName);

        if (notExistingIngredients.Count() != 0)
        {
            List<string> notExistingIngredientNames = await notExistingIngredients.Select(e => e.IngredientName).ToListAsync();
            return (false, $"Ещё не добавлены ингредиенты: {String.Join(", ", notExistingIngredientNames)}");
        }

        List<Ingredient> existingIngredients = await db.Ingredients
            .IntersectBy(model.AvalableIngredients.Keys, x => x.IngredientName)
            .ToListAsync();

        Shop newShop = new()
        {
            Name = model.Name,
            Address = model.Address,
            AvailableIngredients = existingIngredients,
        };

        var addedShop = await db.Shops.AddAsync(newShop);

        var prices = newShop.AvailableIngredients
            .Select(x => new IngredientPriceForShop()
            {
                Ingredient = x,
                Shop = addedShop.Entity,
                Price = model.AvalableIngredients[x.IngredientName],
            })
            .ToList();

        db.IngredientPrices.AddRange(prices);

        addedShop.Entity.IngredientPrices = prices;
        addedShop.State = EntityState.Modified;

        for (int i = 0; i < existingIngredients.Count; i++)
        {
            existingIngredients[i].ShopsWhereAvailable.Add(newShop);
        }

        db.Ingredients.UpdateRange(existingIngredients);

        int affected = await db.SaveChangesAsync();
        return affected == 1 ? (true, null) : (false, "Ошибка добавления");
    }
}
