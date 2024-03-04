using SmartRecipes.DataContext.Recipes.Models;

namespace SmartRecipes.Services.Recomendations.Utilities;

public class SearchTokensGetterByName : ISearchTokensGetter
{
    public IEnumerable<string> GetTokens(Recipe? recipe)
    {
        if (recipe is null) return Enumerable.Empty<string>();
        return recipe.RecipeName.Split(' ');
    }   
}