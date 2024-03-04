using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace SmartRecipes.DataContext.Recipes.Generators.Models;

public sealed class CreateShopModel
{
    [Required]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Длина названия от 2 до 50 символов")]
    public string Name { get; set; }
    [Required]
    [MaxLength(200, ErrorMessage = "Длина адреса до 200 символов")]
    public string Address {  get; set; }
    [Required]  
    public Dictionary<string, int> AvalableIngredients {  get; set; }

}