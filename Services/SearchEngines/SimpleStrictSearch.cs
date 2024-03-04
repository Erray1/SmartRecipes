using Microsoft.EntityFrameworkCore;
using SmartRecipes.DataContext.Recipes;
using SmartRecipes.DataContext.Recipes.Models;
using SmartRecipes.Services.SearchEngines.Utilities;


namespace SmartRecipes.Services.SearchEngines;

public class SimpleStrictSearch : ISearchEngine
{
    private readonly RecipesContext db;

    public SimpleStrictSearch(RecipesContext db)
    {
        this.db = db;
    }

    public IQueryable<Recipe> Search(SearchProperties searchProperty, IEnumerable<string> searchTokens)
    {
        return db.Recipes
            .Where(x => SearchHelper.GetSearchedValues(x, searchProperty)
                .Intersect(searchTokens)
                .Count() >=
                searchTokens
                .Count() / 2)
            .OrderByDescending(x => SearchHelper.GetTokensHitAmount(x, searchProperty, searchTokens));
    }
}
