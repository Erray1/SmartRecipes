using SmartRecipes.DataContext.Recipes.Models;
using SmartRecipes.Services.SearchEngines;

namespace SmartRecipes.Services.SearchEngines.Utilities;

public static class SearchHelper
{
    public static IEnumerable<string> GetSearchedValues(Recipe recipe, SearchProperties searchPreperty)
    {
        switch (searchPreperty)
        {
            case SearchProperties.Name:
                return recipe.RecipeName.Split(" ");
            case SearchProperties.Description:
                return recipe.RecipeDescription.Split(" ");
            case SearchProperties.NameAndDescription:
                return (recipe.RecipeName + " " + recipe.RecipeDescription).Split(" ");
            default:
                throw new ArgumentException("Неверный searchType");
        };
    }

    public static int GetTokensHitAmount(Recipe recipe, SearchProperties searchProperty, IEnumerable<string> searchTokens)
    {
        return GetSearchedValues(recipe, searchProperty).Intersect(searchTokens).Count();
    }
}
