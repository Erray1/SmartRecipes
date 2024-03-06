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
    private readonly IConfiguration appConfig;
    public SearchTokensWorker(RecipesContext db, IConfiguration appConfig)
    {
        this.db = db;
        this.appConfig = appConfig;
    }
    public List<string> GetUniqueTokensOrdered(string userId, SearchProperties searchProperty)
    {
        var userActions = db.Ratings.Where(x => x.UserID == userId);
        if (userActions is null)
        {
            return new();
        }
        ISearchTokensGetter tokensGetter = SearchTokensGetters.CreateNew(searchProperty)!;

        var preferences = calculateTokensPreferencesSorted(getInteractedRecipesValues(userActions), tokensGetter);
        int minimalPreference = calculateMinimalTokensPreference(preferences);

        return preferences
            .Where(x => x.Value >= minimalPreference)
            .Select(x => x.Key)
            .ToList();
        
    }

    private Dictionary<string, int> calculateTokensPreferencesSorted(ImmutableDictionary<IQueryable<Recipe>, int> groupedRecipes, ISearchTokensGetter tokensGetter)
    {
        Dictionary<string, TokenWeightAndCategories> tokensPreferences = new();
        foreach (var pair in groupedRecipes)
        {
            for (int i = 0; i < pair.GetAllRecipes().Count(); i++)
            {
				IEnumerable<string> tokens = tokensGetter.GetTokens(pair.GetRecipe(i));
                for (int j = 0; j < tokens.Count(); j++)
                {
                    if (tokensPreferences.ContainsKey(tokens.ElementAt(j)))
                    {
                        tokensPreferences[tokens.ElementAt(j)].Update(pair.GetPreference(), pair.GetRecipe(i).Category.CategoryName);
                    }
                    tokensPreferences[tokens.ElementAt(j)] = new(pair.GetPreference(), pair.GetRecipe(i).Category.CategoryName);
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

    private ImmutableDictionary<IQueryable<Recipe>, int> getInteractedRecipesValues(IQueryable<Rate> userActions)
    {
        return userActions.GroupBy<Rate, int>(x => appConfig.GetValue<int>($"UserActionsValues:{x.RateType}") +
            appConfig.GetValue<int>($"UserActionsValues:{CreationTimeWorker.GetStringPeriodRepresentation(x.RateCreation)}"))
            .Select(x => new KeyValuePair<IQueryable<Recipe>, int>(
                    x.AsQueryable().Select(r => r.RecipeRated),
                    x.Key
                ))
            .ToImmutableDictionary();
           
    }
}

