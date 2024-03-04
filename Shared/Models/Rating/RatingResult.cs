namespace SmartRecipes.Shared.Models.Rating;


public sealed class RatingResult
{
    public bool IsSuccesful { get; set; }
    public List<string>? Errors { get; set; }
}

