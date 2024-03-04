namespace SmartRecipes.DataContext.Recipes.Models;

public sealed class IngredientAmountForRecipe : EntityModelBase
{
    public Ingredient Ingredient { get; set; } = null!;
    public string IngredientID { get; set; }
    public Recipe Recipe { get; set; } = null!;
    public string RecipeID { get; set; }
    public string Amount { get; set; }
    public IngredientAmountForRecipe()
    {
        
    }
}
