using SmartRecipes.DataContext.Recipes.Models;
using SmartRecipes.Shared.DTO;
using SmartRecipes.Shared.DTO.Shops;


namespace SmartRecipes.DataContext.Repos.Filters.Shops.Filters;

public interface IShopsFilter
{
    public IEnumerable<ShopData> Filter(IQueryable<Shop> query, IEnumerable<string> ingredientsToBuy, ShopsFilterOptions options);
}
