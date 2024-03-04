using SmartRecipes.DataContext.Recipes.Generators.Models;
using SmartRecipes.DataContext.Recipes.Models;
using SmartRecipes.DataContext.Recipes.Generators.Utilities;
using Microsoft.EntityFrameworkCore;

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

    public Task<(bool, string?)> AddImage(CreateImageModel model)
    {
        throw new NotImplementedException();
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
        if (foundImage is null) { return (false, "Не найден файл картинки с таким именем"); }

        Recipe newRecipe = new()
        {
            RecipeName = model.RecipeName,
            RecipeDescription = RecipeDescriptionHandler.ToHTML(model.RecipeDescription),
            Category = foundCategory,
            TimeToCook = model.TimeToCook,
            Rating = model.Rating is null ? default! : model.Rating, // ?
            Ingredients = await db.Ingredients
                .Where(e => model.Ingredients
                    .Select(m => m.Key)
                    .Contains(e.IngredientName))
                    .ToListAsync(),
            RecipeImage = foundImage,
        };
        await db.Recipes.AddAsync(newRecipe);

        await db.IngredientAmounts
            .AddRangeAsync(model.Ingredients
                .Select(x => new IngredientAmountForRecipe()
                {
                    IngredientID = db.Ingredients.FirstOrDefault(y => y.IngredientName == x.Key)?.ID ?? "0",
                    RecipeID = newRecipe.ID,
                    Amount = x.Value
                }));

        foundCategory.RecipesWhereUsed.Add(newRecipe);
        // db.Categories.Update(foundCategory);
        int affected = await db.SaveChangesAsync();
        if (affected == 0) return (false, "Ошибка при обновлении базы данных");

        return (true, null);

    }

    public async Task<(bool, string?)> AddShop(CreateShopModel model)
    {
        var notExistingIngredients = db.Ingredients
             .Where(e => !model.AvalableIngredients.Keys
              .Contains(e.IngredientName));
        if (notExistingIngredients.Count() != 0)
        {
            List<string> notExistingIngredientNames = await notExistingIngredients.Select(e => e.IngredientName).ToListAsync();
            return (false, $"Ещё не добавлены ингредиенты: {String.Join(", ", notExistingIngredientNames)}");
        }

        List<Ingredient> existingIngredients = await db.Ingredients
            .Where(e => model.AvalableIngredients.Keys
            .Contains(e.IngredientName))
            .ToListAsync();

        Shop newShop = new()
        {
            Name = model.Name,
            Address = model.Address,
            AvailableIngredients = existingIngredients,
        };

        await db.Shops.AddAsync(newShop);

        await db.IngredientPrices.AddRangeAsync(newShop.AvailableIngredients
            .Select(x => new IngredientPriceForShop()
            {
                IngredientID = x.ID,
                Price = model.AvalableIngredients[x.IngredientName],
                ShopID = newShop.ID
            }));

        for (int i = 0; i < existingIngredients.Count; i++)
        {
            existingIngredients[i].ShopsWhereAvailable.Add(newShop);
        }

        db.Ingredients.UpdateRange(existingIngredients);

        int affected = await db.SaveChangesAsync();
        return affected == 1 ? (true, null) : (false, "Ошибка добавления");


        //if (model.AvalableIngredientNames.Count() != foundIngredients.Count()) {
        //    var notExistingIngredientNames = model.AvalableIngredientNames
        //        .Where(e => !foundIngredients
        //            .Select(e => e.IngredientName)
        //            .Contains(e));
        //    newIngredients = notExistingIngredientNames.Select(e => new Ingredient()
        //    {
        //        IngredientName = e,
        //        ShopsWhereAvailable = model
        //    })
        //}
    }
}
