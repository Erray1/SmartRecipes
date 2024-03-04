using SmartRecipes.DataContext.Recipes.Models;

namespace SmartRecipes.Services.Recomendations.Utilities;

public interface ISearchTokensGetter
{
    public IEnumerable<string> GetTokens(Recipe? recipe);
}
