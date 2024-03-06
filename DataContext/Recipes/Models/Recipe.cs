using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRecipes.DataContext.Recipes.Models;

public sealed class Recipe : EntityModelBase
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string ID { get; set; }
    public Image RecipeImage { get; set; }
    public string RecipeName { get; set; } = null!;
    public DateTime CreationTime { get; set; }
    public Category Category { get; set; } = null!;
    public string RecipeDescription { get; set; } = null!;
    public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    public ICollection<IngredientAmountForRecipe> IngredientsAmounts { get; set; } = new List<IngredientAmountForRecipe>();
    public ICollection<Rate> Rating { get; set; } = new List<Rate>();
    public float TimeToCook { get; set; }
    public int TimesVisited { get; set; } = 0;

    public Recipe()
    {
        ID = "";
    }
}
