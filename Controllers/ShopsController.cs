using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartRecipes.DataContext.Repos;

namespace SmartRecipes.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class ShopsController : ControllerBase
{
    private readonly IShopsRepository repo;
    public ShopsController(IShopsRepository repo)
    {
        this.repo = repo;
    }
    [HttpGet("{recipeId}")]
    public async Task<IActionResult> GetShopsAndIngredients(string recipeId, [FromQuery]string notPresentIngredients, [FromQuery]string? filter, [FromQuery]string userAddress)
    {
        var data = await repo.GetShopsDataForAsync(recipeId, notPresentIngredients.Split("|"), filter, userAddress);
        if (!data.IsSuccesful)
        {
            return BadRequest(data);
        }
        return Ok(data);
    }
}

