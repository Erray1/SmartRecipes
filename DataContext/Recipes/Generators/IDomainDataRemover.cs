using SmartRecipes.DataContext.Recipes.Generators.Models;

namespace SmartRecipes.DataContext.Recipes.Generators;

public interface IDomainDataRemover
{
    public bool RemoveCategory(CreateCategoryModel model);
    public bool RemoveImage(CreateImageModel model);
    public bool RemoveIngredient(CreateIngredientModel model);
    public bool RemoveShop(CreateShopModel model);
    public bool RemoveRecipe(CreateRecipeModel model);
}
