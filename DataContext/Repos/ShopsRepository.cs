using SmartRecipes.DataContext.Repos.Calculators;
using SmartRecipes.DataContext.Repos.Filters.Shops.Filters;
using SmartRecipes.DataContext.Recipes;
using SmartRecipes.DataContext.Recipes.Models;
using SmartRecipes.Shared.DTO.Shops;

namespace SmartRecipes.DataContext.Repos;

public sealed class ShopsRepository : IShopsRepository
{
    private readonly RecipesContext db;
    private readonly IShopsFilter filter;
    public ShopsRepository(RecipesContext db, IShopsFilter filter)
    {
        this.db = db;
        this.filter = filter;
    }
    public async Task<ShopsDto> GetShopsDataForAsync(string recipeID, IEnumerable<string> ingredientsToBuy, string? shopsFilter, string userAddress)
    {
        Recipe? recipeFound = await db.Recipes.FindAsync(recipeID);
        if (recipeFound is null)
        {
            return new()
            {
                IsSuccesful = false,
                Errors = new() { "Не найден рецепт с данным ID" },
                Content = new()
            };
        }

        var shopData = filter.Filter(db.Shops,
            ingredientsToBuy,
            new ShopsFilterOptions() { FilterString = shopsFilter, UserAddress = userAddress })
            .ToList();

        return new()
        {
            IsSuccesful = true,
            Content = shopData,
            FinalPrice = PriceOfIngredientsCalculator.Calculate(shopData)
        };

    }
}
