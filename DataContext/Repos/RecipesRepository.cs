﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartRecipes.DataContext.Recipes;
using SmartRecipes.DataContext.Repos.Utilities;
using SmartRecipes.Services.Rating;
using SmartRecipes.Services.SearchEngines;
using SmartRecipes.Shared.DTO.Recipes;

namespace SmartRecipes.DataContext.Repos;

public sealed class RecipesRepository : IRecipesRepository
{
    private readonly RecipesContext db;
    private readonly ISearchEngine searchEngine;
    public RecipesRepository(RecipesContext db, SimpleStrictSearch searchEngine)
    {
        this.db = db;
        this.searchEngine = searchEngine;
    }

    public async Task<RecipeListDto<RecipePreviewData>> GetLikedRecipesPagedAsync(string userId, int itemsPerPage, int currentPage)
    {
        var recipesSelected = await db.Recipes
            .Where(x => x.Rating.Any(y => y.UserID == userId))
            .Skip(itemsPerPage)
            .Take((currentPage - 1) * itemsPerPage)
            .ToListAsync();

        if (recipesSelected is null || recipesSelected.Count() == 0)
        {
            return new()
            {
                IsSuccesful = false,
                Errors = new() { "Вы не добавили ни одного рецепта в избранное" },
                Content = new() { }
            };
        }
        return new()
        {
            IsSuccesful = true,
            Content = recipesSelected.Select(x => RecipesDTOMapper.ToDTOPreview(x)).ToList()
        };
    }

    public async Task<RecipeListDto<RecipePreviewData>> GetPopularRecipesPagedAsync(int itemsPerPage, int currentPage)
    {
        var recipesSelected = await db.Recipes
            .OrderByDescending(x => x.TimesVisited)
            .ThenByDescending(x => x.Rating.Where(y => y.RateType == "like"))
            .ThenBy(x => x.Rating.Where(y => y.RateType == "dislike"))
            .Skip(itemsPerPage)
            .Take((currentPage - 1) * itemsPerPage)
            .ToListAsync();

        if (recipesSelected is null || recipesSelected.Count() == 0)
        {
            return new()
            {
                IsSuccesful = false,
                Errors = new() { "Не найдено рецептов с данной выборкой" },
                Content = new() { }
            };
        }
        return new()
        {
            IsSuccesful = true,
            Content = recipesSelected.Select(x => RecipesDTOMapper.ToDTOPreview(x)).ToList()
        };
    }

    public async Task<RecipeDto<RecipeData>> GetRecipeByIDAsync(string id)
    {
        var recipeFound = await db.Recipes.FindAsync(id);
        if (recipeFound is null)
        {
            return new()
            {
                IsSuccesful = false,
                Errors = new() { "Не найдено рецепта с таким ID" },
                Content = new() { }
            };
        }
        int affected = await db.SaveChangesAsync();
        if (affected == 0)
        {
            return new()
            {
                IsSuccesful = false,
                Errors = new() { "Ошибка обновления базы" },
                Content = new() { }
            };
        }

        return new()
        {
            IsSuccesful = true,
            Content = RecipesDTOMapper.ToDTOFull(recipeFound)
        };
    }

    public async Task<RecipeListDto<RecipeShortenedData>> GetRecipesByIDAsync(IEnumerable<string> IDs)
    {
        var recipesFound = await db.Recipes.IntersectBy(IDs, x => x.ID).ToListAsync();
        if (recipesFound.Count() < IDs.Count())
        {
            return new()
            {
                IsSuccesful = false,
                Errors = new() { "Не найдены некоторые рецепты" },
                Content = new()
            };
        }

        return new()
        {
            IsSuccesful = true,
            Content = recipesFound.Select(x => RecipesDTOMapper.ToDTOShortened(x)).ToList()
        };
    }
    public async Task<RecipeListDto<RecipePreviewData>> GetRecipesByIDPagedAsync(IEnumerable<string> IDs, int itemsPerPage, int currentPage)
    {
        var recipesFound = await db.Recipes
            .IntersectBy(IDs, x => x.ID)
            .Skip(itemsPerPage * (currentPage - 1))
            .Take(itemsPerPage)
            .ToListAsync();

        if (recipesFound.Count() < IDs.Count())
        {
            return new()
            {
                IsSuccesful = false,
                Errors = new() { "Не найдены некоторые рецепты" },
                Content = new()
            };
        }

        return new()
        {
            IsSuccesful = true,
            Content = recipesFound.Select(x => RecipesDTOMapper.ToDTOPreview(x)).ToList()
        };
    }

    public RecipeListDto<RecipeShortenedData> SearchFirstRecipes(int itemsCount, string searchString)
    {
        var recipesSelected = searchEngine
            .Search(SearchProperties.Name, searchString.Split(" "))
            .Take(itemsCount);

        if (recipesSelected is null || recipesSelected.Count() == 0)
        {
            return new()
            {
                IsSuccesful = false,
                Errors = new() { "Не найдено таких рецептов" },
                Content = new() { }
            };
        }
        return new()
        {
            IsSuccesful = true,
            Content = recipesSelected.Select(x => RecipesDTOMapper.ToDTOShortened(x)).ToList()
        };
    }

    public RecipeListDto<RecipePreviewData> SearchRecipesPaged(int itemsPerPage, int currentPage, string searchString)
    {
        var recipesSelected = searchEngine
            .Search(SearchProperties.Name, searchString.Split(" "))
            .Skip((currentPage - 1) * itemsPerPage)
            .Take(itemsPerPage);

        if (recipesSelected is null || recipesSelected.Count() == 0)
        {
            return new()
            {
                IsSuccesful = false,
                Errors = new() { "Не найдено таких рецептов" },
                Content = new() { }
            };
        }
        return new()
        {
            IsSuccesful = true,
            Content = recipesSelected.Select(x => RecipesDTOMapper.ToDTOPreview(x)).ToList()
        };
    }
}
