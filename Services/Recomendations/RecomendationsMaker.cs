using SmartRecipes.DataContext.Recipes;
using SmartRecipes.DataContext.Recipes.Models;
using SmartRecipes.DataContext.Users.Models;
using SmartRecipes.Services.Recomendations.Utilities;
using SmartRecipes.Services.SearchEngines;

namespace SmartRecipes.Services.Recomendations;

public sealed class RecomendationsMaker
{
    private readonly ISearchEngine searchEngine;
    private readonly SearchTokensWorker searchTokensWorker;
    public RecomendationsMaker(SimpleLargeInputSearch searchEngine, SearchTokensWorker searchTokensWorker)
    {
        this.searchTokensWorker = searchTokensWorker;
        this.searchEngine = searchEngine;
    }
    public IQueryable<Recipe> GetRecomendationsQuery(User user)
    {
        var uniqueTokens = searchTokensWorker.GetUniqueTokensOrdered(RecipesInteractedByUser.GetFromUser(user), SearchProperties.Name);
        var recipesSearched = searchEngine.Search(SearchProperties.Name, uniqueTokens);

        return recipesSearched;
    }
}
