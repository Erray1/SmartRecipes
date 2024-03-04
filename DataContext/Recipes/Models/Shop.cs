using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRecipes.DataContext.Recipes.Models;

public sealed class Shop : EntityModelBase
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string ID { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public ICollection<Ingredient> AvailableIngredients { get; set; } = new List<Ingredient>();
    public ICollection<IngredientPriceForShop> IngredientPrices { get; set; } = new List<IngredientPriceForShop>();
    public Shop()
    {
        
    }
}
