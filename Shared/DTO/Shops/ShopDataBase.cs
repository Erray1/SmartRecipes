using SmartRecipes.Shared.DTO;

namespace SmartRecipes.Shared.DTO.Shops;

public abstract class ShopDataBase : DataBase
{
    public int TimeToTravelMinutes { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public List<IngredientPriceData> Ingredients { get; set; }
}
