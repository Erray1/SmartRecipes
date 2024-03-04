using SmartRecipes.Shared.DTO;

namespace SmartRecipes.Shared.DTO.Shops;

public class ShopsDto : DtoBase
{
    public List<ShopData> Content { get; set; }
    public float FinalPrice { get; set; }
}