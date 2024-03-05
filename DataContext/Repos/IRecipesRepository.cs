using SmartRecipes.Shared.DTO.Recipes;

namespace SmartRecipes.DataContext.Repos;
public interface IRecipesRepository
{
    public Task<RecipeListDto<RecipePreviewData>> GetPopularRecipesPagedAsync(int itemsPerPage, int currentPage);
    public RecipeListDto<RecipePreviewData> SearchRecipesPaged(int itemsPerPage, int currentPage, string searchToken);
    public RecipeListDto<RecipeShortenedData> SearchFirstRecipes(int itemsCount, string searchToken);
    public Task<RecipeListDto<RecipeShortenedData>> GetRecipesByIDAsync(IEnumerable<string> IDs);
    public Task<RecipeListDto<RecipePreviewData>> GetRecipesByIDPagedAsync(IEnumerable<string> IDs, int itemsPerPage, int currentPage);
    public Task<RecipeDto<RecipeData>> GetRecipeByIDAsync(string id);
}

