using SmartRecipes.DataContext.Recipes.Generators.Models;

namespace SmartRecipes.DataContext.Recipes.Generators;

public interface IDomainDataAdder
{
    public Task<(bool, string?)> AddCategory(CreateCategoryModel model);
    public Task<(bool, string?)> AddIngredient(CreateIngredientModel model);
    public Task<(bool, string?)> AddShop(CreateShopModel model);
    public Task<(bool, string?)> AddRecipe(CreateRecipeModel model);
    public Task<(bool, string?)> AddImage(CreateImageModel model);
}
