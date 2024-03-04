namespace SmartRecipes.Shared.DTO.Recipes;
public class RecipePreviewData : RecipeDataBase
{
    public int IngedientsCount { get; set; }
    public float TimeToCook { get; set; }
    public Dictionary<string, int> Rating { get; set; } = new();
}

