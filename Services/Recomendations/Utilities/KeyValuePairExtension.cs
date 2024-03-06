using SmartRecipes.DataContext.Recipes.Models;
using System.Runtime.CompilerServices;

namespace SmartRecipes.Services.Recomendations.Utilities;

public static class KeyValuePairExtensions
{
    public static IQueryable<Recipe> GetAllRecipes(this KeyValuePair<IQueryable<Recipe>, int> pair)
    {
        return pair.Key;
    }
    public static Recipe GetRecipe(this KeyValuePair<IQueryable<Recipe>, int> pair, int index)
    {
        return pair.Key.ElementAt(index);
    }
    public static int GetPreference(this KeyValuePair<IQueryable<Recipe>, int> pair)
    {
        return pair.Value;
    }
}
