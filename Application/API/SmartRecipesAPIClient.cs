using SmartRecipes.Shared.DTO.Recipes;
using SmartRecipes.Shared.DTO.Shops;
using SmartRecipes.Shared.Models.Rating;

namespace SmartRecipes.Application.API;

public sealed class SmartRecipesAPIClient
{
    private readonly HttpClient httpClient;
    public SmartRecipesAPIClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    public async Task<RecipeListDto<RecipePreviewData>> GetPopular(int itemsPerPage, int currentPage)
    {
        return (await httpClient.GetFromJsonAsync<RecipeListDto<RecipePreviewData>>($"api/recipes/get_popular?itemsPerPage={itemsPerPage}&currentPage={currentPage}"))!;
    }

    public async Task<RecipeListDto<RecipePreviewData>> GetLiked(int itemsPerPage, int currentPage)
    {
        return (await httpClient.GetFromJsonAsync<RecipeListDto<RecipePreviewData>>($"api/recipes/get-liked?itemsPerPage={itemsPerPage}&currentPage={currentPage}"))!;
    }
    
    public async Task<RecipeListDto<RecipePreviewData>> GetRecommendations(int itemsPerPage, int currentPage)
    {
        return (await httpClient.GetFromJsonAsync<RecipeListDto<RecipePreviewData>>($"api/recipes/get-recommendations?itemsPerPage={itemsPerPage}&currentPage={currentPage}"))!;
    }

    public async Task<RecipeListDto<RecipeShortenedData>> GetLatest()
    {
        return (await httpClient.GetFromJsonAsync<RecipeListDto<RecipeShortenedData>>($"api/recipes/get-latest"))!;
    }

    public async Task<RecipeListDto<RecipeShortenedData>> GetSearchResultShortened(int itemsCount, string searchString)
    {
        return (await httpClient.GetFromJsonAsync<RecipeListDto<RecipeShortenedData>>($"api/recipes/get-list-shortened?itemsCount={itemsCount}&search={searchString}"))!;
    }

    public async Task<RecipeListDto<RecipePreviewData>> GetSearchResultFull(int itemsPerPage, int currentPage, string searchString)
    {
        return (await httpClient.GetFromJsonAsync<RecipeListDto<RecipePreviewData>>($"api/recipes/get-list-shortened?itemsPerPage={itemsPerPage}&currentPage={currentPage}&search={searchString}"))!;
    }
    public async Task<RecipeDto<RecipeData>> GetRecipe(string id)
    {
        return (await httpClient.GetFromJsonAsync<RecipeDto<RecipeData>>($"api/recipes/{id}"))!;
    }
    public async Task<ShopsDto> GetShopsData(string recipeId, IEnumerable<string> notPresentIngredients, string userAddress = "", string filter = "")
    {
        var ingredientsString = String.Join("|", notPresentIngredients);
        return (await httpClient.GetFromJsonAsync<ShopsDto>($"spi/shops/{recipeId}?notPresentIngredients={ingredientsString}&filter={filter}&userAddress={userAddress}"))!;
    }
    public async Task<RatingResult> RateRecipe(string recipeId, RatingModel model)
    {
        var response = await httpClient.PatchAsJsonAsync($"api/rating/{recipeId}", model);
        return (await response.Content.ReadFromJsonAsync<RatingResult>())!;
    }
    
}
