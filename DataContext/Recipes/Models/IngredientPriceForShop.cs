namespace SmartRecipes.DataContext.Recipes.Models;

public sealed class IngredientPriceForShop : EntityModelBase
{
    public Ingredient Ingredient { get; set; } = null!;
    //public string IngredientID { get; set; }
    public Shop Shop { get; set; } = null!;
    //public string ShopID { get; set; }
    
    public float Price { get; set; }
    public IngredientPriceForShop()
    {
        
    }
}
