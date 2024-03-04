using SmartRecipes.DataContext.Recipes;
using SmartRecipes.DataContext.Recipes.Models;
using SmartRecipes.Services.SearchEngines.Utilities;

namespace SmartRecipes.Services.SearchEngines;
public class SimpleLargeInputSearch : ISearchEngine
{
    private readonly RecipesContext db;
    public SimpleLargeInputSearch(RecipesContext db)
    {
        this.db = db;
    }
    public IQueryable<Recipe> Search(SearchProperties searchProperty, IEnumerable<string> searchTokens)
    {
        return searchTokens
            .Select(x =>
                db.Recipes.Where(r =>
                    SearchHelper.GetSearchedValues(r, searchProperty).Contains(x)
                )
            )
            .Aggregate((x1, x2) => x1.UnionBy(x2, x => x.ID)); // Скорее всего, это не работает
    }
}

