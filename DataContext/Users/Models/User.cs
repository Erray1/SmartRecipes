using Microsoft.AspNetCore.Identity;
using SmartRecipes.Server.DataContext.Utilities;


namespace SmartRecipes.DataContext.Users.Models;

public class User : IdentityUser
{
    public DefinedLengthCollection<string> LastVisitedRecipesIDs { get; } = new(7);
    public ICollection<string> LikedRecipesIDs { get; } = new List<string>();
    public ICollection<string> DislikedRecipesIDs { get; } = new List<string>();
}