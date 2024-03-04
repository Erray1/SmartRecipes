using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations.Schema;


namespace SmartRecipes.DataContext.Recipes.Models;

public sealed class Ingredient : EntityModelBase
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string ID { get; set; }
    public string IngredientName { get; set; } = string.Empty;
    public ICollection<Shop> ShopsWhereAvailable { get;} = new List<Shop>();
    public ICollection<IngredientPriceForShop> PriceInShops { get;} = new List<IngredientPriceForShop>();
    public ICollection<Recipe> RecipesWhereUsed { get;} = new List<Recipe>();
    public ICollection<IngredientAmountForRecipe> AmountsForRecipes {get;} = new List<IngredientAmountForRecipe>();
    public Ingredient()
    {
        
    }
}
