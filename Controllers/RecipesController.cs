using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartRecipes.DataContext.Repos;
using SmartRecipes.DataContext.Users.Models;
using SmartRecipes.Services.Rating;
using SmartRecipes.Services.Recomendations;
using System.Security.Claims;

namespace SmartRecipes.Controllers;


[Route("api/[controller]")]
[ApiController]
public sealed class RecipesController : ControllerBase
{
    private readonly IRecipesRepository repo;
    public RecipesController(IRecipesRepository repo)
    {
        this.repo = repo;
    }
    [HttpGet("get-popular")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetPopularRecipes([FromQuery] int itemsPerPage, [FromQuery] int currentPage)
    {
        var response = await repo.GetPopularRecipesPagedAsync(itemsPerPage, currentPage);
        if (!response.IsSuccesful)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpGet("get-favourite")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [Authorize]
    public async Task<IActionResult> GetFavouriteRecipes(UserManager<User> userManager)
    {
        User user = (await userManager.FindByIdAsync(User.FindFirst(ClaimTypes.NameIdentifier)!.Value))!;
        var response = await repo.GetRecipesByIDAsync(user.LikedRecipesIDs);
        
        if (!response.IsSuccesful)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [HttpGet("get-recommendations")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [Authorize]
    public async Task<IActionResult> GetRecommendedRecipes(RecomendationsService recs)
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var response = await recs.GetRecomendationsPagedAsync(userId!,
            Convert.ToInt32(Request.Query["itemsPerPage"]),
            Convert.ToInt32(Request.Query["currentPage"]));

        if (!response.IsSuccesful)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [HttpGet("get-latest")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [Authorize]
    public async Task<IActionResult> GetLatestRecipes(UserManager<User> userManager)
    {
		string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        User user = (await userManager.FindByIdAsync(userId))!;
		var data = await repo.GetRecipesByIDAsync(user.LastVisitedRecipesIDs.Items);
        if (!data.IsSuccesful)
        {
            return NotFound(data);
        }
        return Ok(data);
    }

    [HttpGet("get-list-shortened")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public IActionResult SearchShortenedRecipes([FromQuery] int itemsCount, [FromQuery] string search)
    {
        var data = repo.SearchFirstRecipes(itemsCount, search);
        if (!data.IsSuccesful)
        {
            return NotFound(data);
        }
        return Ok(data);
    }

    [HttpGet("get-list")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public IActionResult SearchRecipes([FromQuery] int itemsPerPage, [FromQuery] int currentPage, [FromQuery] string search)
    {
        var data = repo.SearchRecipesPaged(itemsPerPage, currentPage, search);
        if (!data.IsSuccesful)
        {
            return NotFound(data);
        }
        return Ok(data);
    }

    [HttpGet("{recipeId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetRecipe(UserActionService actionService)
    {
        var recipeId = Request.Path.Value!.Split("/").First()!; // Хз, работает ли
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

		var data = await repo.GetRecipeByIDAsync(recipeId);
        if (!data.IsSuccesful)
        {
            return NotFound(data);
        }
        if (User.Identity is not null && User.Identity.IsAuthenticated)
        {
            await actionService.SaveVisitAsync(recipeId, userId);
        }
        return Ok(data);
    }
}

