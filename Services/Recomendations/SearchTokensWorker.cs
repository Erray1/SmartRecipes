using SmartRecipes.DataContext.Recipes;
using System.Reflection;
using SmartRecipes.Services.Recomendations.Utilities;
using System.Collections.Immutable;
using SmartRecipes.DataContext.Recipes.Models;
using SmartRecipes.Services.SearchEngines;

namespace SmartRecipes.Services.Recomendations;

public class SearchTokensWorker
{
    private readonly RecipesContext db;
    public SearchTokensWorker(RecipesContext db)
    {
        this.db = db;
    }
    public List<string> GetUniqueTokensOrdered(RecipesInteractedByUser recipes, SearchProperties searchProperty)
    {
        ISearchTokensGetter tokensGetter = SearchTokensGetters.CreateNew(searchProperty)!;

        var preferences = calculateTokensPreferencesSorted(getActionIDsAndValues(recipes), tokensGetter);
        int minimalPreference = calculateMinimalTokensPreference(preferences);

        return preferences
            .Where(x => x.Value >= minimalPreference)
            .Select(x => x.Key)
            .ToList();
        
    }

    private Dictionary<string, int> calculateTokensPreferencesSorted(ImmutableDictionary<ICollection<string>, int> idsValues, ISearchTokensGetter tokensGetter)
    {
        Dictionary<string, TokenWeightAndCategories> tokensPreferences = new();
        foreach (var pair in idsValues)
        {
            for (int i = 0; i < pair.Key.Count; i++)
            {
                Recipe? recipe = db.Recipes.Find(pair.Key.ElementAt(i));

				IEnumerable<string> tokens = tokensGetter.GetTokens(recipe);
                for (int j = 0; j < tokens.Count(); j++)
                {
                    if (tokensPreferences.ContainsKey(tokens.ElementAt(j)))
                    {
                        tokensPreferences[tokens.ElementAt(j)].Update(pair.Value, recipe!.Category.CategoryName);
                    }
                    tokensPreferences[tokens.ElementAt(j)] = new(pair.Value, recipe!.Category.CategoryName);
				}
            }
        }
        return tokensPreferences
            .OrderByDescending(x => x.Value.Weight)
            .ThenByDescending(x => x.Value.GetMostPreferencedCategory()) // ?
            .Select(x => new KeyValuePair<string, int>(x.Key, x.Value.Weight))
            .ToDictionary();
        
	}

    private int calculateMinimalTokensPreference(Dictionary<string, int> preferences)
    {
        return Convert.ToInt32(preferences.Values.Average());
    }
    private ImmutableDictionary<ICollection<string>, int> getActionIDsAndValues(RecipesInteractedByUser recipes)
    {
        Type type = recipes.GetType();
        PropertyInfo[] properties = type.GetProperties();
        return properties
            .Select(prop =>
            new KeyValuePair<ICollection<string>, int>(
                (ICollection<string>)prop.GetValue(recipes)!,
                prop.GetCustomAttribute<ActionTypeValueAttribute>()!.ActionValue
                )
            ).ToImmutableDictionary();
    }
}

