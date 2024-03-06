
using Microsoft.EntityFrameworkCore;
using SmartRecipes.Services.SearchEngines;
using SmartRecipes.Shared.DTO.Recipes;

namespace SmartRecipes.Services.Recomendations;

public sealed class RecomendationsService
{
    private readonly ISearchEngine searchEngine;
    private readonly SearchTokensWorker searchTokensWorker;
    public RecomendationsService(SimpleLargeInputSearch searchEngine, SearchTokensWorker searchTokensWorker)
    {
        this.searchEngine = searchEngine;
        this.searchTokensWorker = searchTokensWorker;
    }
    public async Task<RecipeListDto<RecipePreviewData>> GetRecomendationsPagedAsync(string userId, int itemsPerPage, int currentPage)
    {
        var uniqueTokens = searchTokensWorker.GetUniqueTokensOrdered(userId, SearchProperties.Name);
        var recipesQuery = searchEngine.Search(SearchProperties.Name, uniqueTokens);

        var data = await recipesQuery
            .Skip((currentPage - 1) * itemsPerPage)
            .Take(itemsPerPage)
            .ToListAsync();

        return new()
        {
            IsSuccesful = true,
            Content = data.Select(x => RecipesDTOMapper.ToDTOPreview(x)).ToList()
        };
    }
}
