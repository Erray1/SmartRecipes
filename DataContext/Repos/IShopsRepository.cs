using SmartRecipes.Shared.DTO.Shops;

namespace SmartRecipes.DataContext.Repos;

public interface IShopsRepository
{
    public Task<ShopsDto> GetShopsDataForAsync(string recipeID, IEnumerable<string> notPresentIngredientIds, string? shopsFilter, string userAddress);
}