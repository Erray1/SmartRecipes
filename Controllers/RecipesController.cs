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
    [ResponseCache(Duration = 86400, Location = ResponseCacheLocation.Any, VaryByQueryKeys = ["currentPage"])]
    public async Task<IActionResult> GetPopularRecipes([FromQuery] int itemsPerPage, [FromQuery] int currentPage)
    {
        var response = await repo.GetPopularRecipesPagedAsync(itemsPerPage, currentPage);
        if (!response.IsSuccesful)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpGet("get-liked")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [Authorize]
    public async Task<IActionResult> GetLikedRecipes(UserManager<User> userManager)
    {
        User user = (await userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email)!.Value))!;
        var response = await repo.GetLikedRecipesPagedAsync(user.Id, 
            Convert.ToInt32(Request.Query["itemsPerPage"]),
            Convert.ToInt32(Request.Query["currentPage"]));
        
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
    [ResponseCache(Duration = 43200, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> GetRecommendedRecipes(RecomendationsService recs, UserManager<User> userManager)
    {
        User user = (await userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email)!.Value))!;

        var response = await recs.GetRecomendationsPagedAsync(user.Id,
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
        User user = (await userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email)!.Value))!;

        var data = await repo.GetRecipesByIDAsync((Request.Query["ids"]).ToString().Split('|'));
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

