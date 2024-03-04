using Microsoft.AspNetCore.Identity;
using SmartRecipes.DataContext.Recipes;
using SmartRecipes.DataContext.Recipes.Models;
using SmartRecipes.DataContext.Users.Models;
using SmartRecipes.Shared.Models.Rating;

namespace SmartRecipes.Services.Rating;

public class UserActionService
{
    private readonly RecipesContext db;
    private readonly UserManager<User> userManager;

	public UserActionService(RecipesContext db, UserManager<User> userManager)
    {
        this.db = db;
        this.userManager = userManager;
    }
    public async Task<(int, RatingResult)> SaveRateAsync(string recipeId, string userId, RatingModel model)
    {
        Recipe? recipeFound = await db.Recipes.FindAsync(recipeId);
        User userFound = (await userManager.FindByIdAsync(userId))!;
        if (recipeFound is null)
        {
            return (StatusCodes.Status404NotFound, new()
            {
                IsSuccesful = false,
                Errors = new() { $"Рецепт с ID {recipeId} не найден"}
            });
        }

        if (model.Type == "like" && userFound.LikedRecipesIDs.Contains(recipeId) && model.IsPositive ||
            model.Type == "dislike" && userFound.DislikedRecipesIDs.Contains(recipeId) && model.IsPositive ||
            model.Type == "like" && !userFound.LikedRecipesIDs.Contains(recipeId) && !model.IsPositive ||
            model.Type == "dislike" && !userFound.DislikedRecipesIDs.Contains(recipeId) && !model.IsPositive)
        {
            return (StatusCodes.Status400BadRequest, new()
            {
                IsSuccesful = false,
                Errors = new() { String.Format("Оценка {0} {1}", model.Type, model.IsPositive ? "уже выставлена" : "не может быть снята")}
            });
        }

        switch (model.Type)
        {
            case "like":
                userFound.LikedRecipesIDs.Add(recipeId);
                recipeFound.Rating["likes"]++;
                break;
            case "dislike":
                userFound.LikedRecipesIDs.Add(recipeId);
                recipeFound.Rating["dislikes"]++;
                break;
        }

        bool affectedRecipe = (await db.SaveChangesAsync()) == 1;
        bool affectedUser = (await userManager.UpdateAsync(userFound)).Succeeded;

        if (!affectedUser || !affectedRecipe)
        {
            return (StatusCodes.Status500InternalServerError, new()
            {
                IsSuccesful = false,
                Errors = new() { "Ошибка обновления" }
            });
        }

        return (StatusCodes.Status204NoContent, new()
        {
            IsSuccesful = true
        });
    }

    public async Task<bool> SaveVisitAsync(string recipeId, string userId)
    {
		Recipe? recipeFound = await db.Recipes.FindAsync(recipeId);
		User userFound = (await userManager.FindByIdAsync(userId))!;

        if (recipeFound is null) return false;

        recipeFound.TimesVisited++;
        userFound.LastVisitedRecipesIDs.Add(recipeFound.ID);

        bool affectedRecipes = (await db.SaveChangesAsync()) == 1;
        bool affectedUsers = (await userManager.UpdateAsync(userFound)).Succeeded;

        return affectedRecipes && affectedUsers;
	}
}