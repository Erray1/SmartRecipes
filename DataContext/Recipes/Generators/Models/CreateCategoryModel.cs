using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SmartRecipes.DataContext.Recipes.Generators.Models;

public sealed class CreateCategoryModel
{
    [Required]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "Длина названия от 5 до 50 символов")]
    public string Name { get; set; }
}