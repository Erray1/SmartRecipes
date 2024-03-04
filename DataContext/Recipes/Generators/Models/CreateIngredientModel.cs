using System.ComponentModel.DataAnnotations;

namespace SmartRecipes.DataContext.Recipes.Generators.Models;

public sealed class CreateIngredientModel
{
    [Required]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Длина названия от 5 до 50 символов")]
    public string Name { get; set; }

}
