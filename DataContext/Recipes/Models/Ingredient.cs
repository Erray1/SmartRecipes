using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations.Schema;


namespace SmartRecipes.DataContext.Recipes.Models;

public sealed class Ingredient : EntityModelBase
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string ID { get; set; }
    public string IngredientName { get; set; } = string.Empty;
    public int Price { get; set; }
    public ICollection<Shop> ShopsWhereAvailable { get; set; } = new List<Shop>();
    public ICollection<IngredientPriceForShop> PriceInShops { get; set; } = new List<IngredientPriceForShop>();
    public ICollection<Recipe> RecipesWhereUsed { get; set; } = new List<Recipe>();
    public ICollection<IngredientAmountForRecipe> AmountsForRecipes {get; set; } = new List<IngredientAmountForRecipe>();
    public Ingredient()
    {
        
    }
}
