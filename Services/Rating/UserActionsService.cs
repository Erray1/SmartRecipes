using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartRecipes.DataContext.Recipes;
using SmartRecipes.DataContext.Recipes.Models;
using SmartRecipes.DataContext.Users.Models;
using SmartRecipes.Shared.Models.Rating;

namespace SmartRecipes.Services.Rating;

public class UserActionService
{
    private readonly RecipesContext db;

	public UserActionService(RecipesContext db)
    {
        this.db = db;
    }
    public async Task<(int, RatingResult)> SaveRateAsync(string recipeId, string userId, RatingModel model)
    {
        Recipe? recipeFound = await db.Recipes.FindAsync(recipeId);
        if (recipeFound is null)
        {
            return (StatusCodes.Status404NotFound, new()
            {
                IsSuccesful = false,
                Errors = new() { $"Рецепт с ID {recipeId} не найден"}
            });
        }

        var foundRate = await db.Ratings.SingleOrDefaultAsync(x => x.UserID == userId && x.RecipeRatedID == recipeFound.ID);

        if (model.IsPositive)
        {
            if (foundRate is not null && foundRate.RateType == model.Type)
            {
                return (StatusCodes.Status400BadRequest, new()
                {
                    IsSuccesful = false,
                    Errors = new() { $"Оценка {model.Type} уже выставлена" }
                });
            }
            else if (foundRate is not null && foundRate.RateType != model.Type)
            {
                foundRate.RateType = model.Type;
                foundRate.RateCreation = DateTime.Now;
            }
            else
            {
                Rate newRate = new()
                {
                    RecipeRated = recipeFound,
                    UserID = userId,
                    RateType = model.Type
                };
                var addedRate = await db.Ratings.AddAsync(newRate);
                recipeFound.Rating.Add(addedRate.Entity);
            }
        }
        else
        {
            if (foundRate is null)
            {
                return (StatusCodes.Status400BadRequest, new()
                {
                    IsSuccesful = false,
                    Errors = new() { $"Оценка {model.Type} не может быть снята тк рецепт не был оценён пользователем" }
                });
            }
            else if (foundRate is not null && foundRate.RateType == model.Type)
            {
                recipeFound.Rating.Remove(foundRate);
            }
            else
            {
                return (StatusCodes.Status400BadRequest, new()
                {
                    IsSuccesful = false,
                    Errors = new() { $"Оценка {model.Type} не может быть снята тк она не совпадает с существующей оценкой {foundRate!.RateType}" }
                });
            }
        }

        bool affected = (await db.SaveChangesAsync()) == 1;

        if (!affected)
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

        if (recipeFound is null) return false;

        recipeFound.TimesVisited++;

        bool affectedRecipes = (await db.SaveChangesAsync()) == 1;

        return affectedRecipes;
	}
}