using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRecipes.DataContext.Recipes.Models;

public sealed class Rate
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string RateID { get; set; }
    public string UserID { get; set; }
    public Recipe RecipeRated { get; set; }
    public string RecipeRatedID { get; set; }
    public DateTime RateCreation { get; set; }
    public string RateType { get; set; }
}

