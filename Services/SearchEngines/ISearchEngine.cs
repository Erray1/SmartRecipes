using SmartRecipes.DataContext.Recipes.Models;

namespace SmartRecipes.Services.SearchEngines;

public interface ISearchEngine
{
    public IQueryable<Recipe> Search(SearchProperties searchProperty, IEnumerable<string> searchTokens);
}