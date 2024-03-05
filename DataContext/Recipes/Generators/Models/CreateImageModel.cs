using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SmartRecipes.DataContext.Recipes.Generators.Models;

public sealed class CreateImageModel
{
    [Required]
    //[RegularExpression(".{5, 40}[.]png", ErrorMessage = "Неверная длина (5-40) или не в формате PNG")] // Check
    public string Name { get; set; }
}
