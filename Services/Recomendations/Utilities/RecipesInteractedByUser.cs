using SmartRecipes.DataContext.Users.Models;

namespace SmartRecipes.Services.Recomendations.Utilities;

public class RecipesInteractedByUser
{
    [ActionTypeValue(3)]
    public ICollection<string> VisitedRecipesIDs { get; set; } = new List<string>();

	[ActionTypeValue(5)]
	public ICollection<string> LikedRecipesIDs { get; set; } = new List<string>();

	[ActionTypeValue(-4)]
	public ICollection<string> DislikedRecipesIDs { get; set; } = new List<string>();

    public static RecipesInteractedByUser GetFromUser(User user)
    {
        return new()
        {
            VisitedRecipesIDs = user.LastVisitedRecipesIDs, // .Items ??
            LikedRecipesIDs = user.LikedRecipesIDs,
            DislikedRecipesIDs = user.DislikedRecipesIDs
        };
    }
}
