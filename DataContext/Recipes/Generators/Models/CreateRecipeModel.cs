using System.ComponentModel.DataAnnotations;

namespace SmartRecipes.DataContext.Recipes.Generators.Models;

public sealed class CreateRecipeModel
{
    [Required]
    [MaxLength(50, ErrorMessage = "Превышена длина - 50 символов")]
    [MinLength(5, ErrorMessage = "Недостаточная длина - 5 символов")]
    public string RecipeName { get; set; }
    
    [Required]
    [MaxLength(30, ErrorMessage = "Превышена длина - 30 символов")]
    [MinLength(5, ErrorMessage = "Недостаточная длина - 5 символов")]
    public string CategoryName { get; set; }

    [Required]
    public string ImageName { get; set; }

    [Required]
    [MaxLength(6400, ErrorMessage = "Превышена длина - 6400 символов")]
    [MinLength(100, ErrorMessage = "Недостаточная длина - 100 символов")]
    public string RecipeDescription {  get; set; }
    [Required]
    public Dictionary<string, string> Ingredients { get; set; }
    public Dictionary<string, int> Rating { get; set; }
    [Required]
    public int TimeToCook { get; set; }
}
