using SmartRecipes.DataContext.Recipes.Models;

namespace SmartRecipes.Shared.DTO.Recipes;

public static class RecipesDTOMapper
{
    public static RecipeShortenedData ToDTOShortened(Recipe recipe)
    {
        return new RecipeShortenedData
        {
            ID = recipe.ID,
            Image = recipe.RecipeImage.ImageURL,
            Name = recipe.RecipeName
        };
    }
    public static RecipePreviewData ToDTOPreview(Recipe recipe)
    {
        return new RecipePreviewData
        {
            ID = recipe.ID,
            Image = recipe.RecipeImage.ImageURL,
            IngedientsCount = recipe.Ingredients.Count(),
            Name = recipe.RecipeName,
            Rating = recipe.Rating,
            TimeToCook = recipe.TimeToCook
        };
    }
    public static RecipeData ToDTOFull(Recipe recipe)
    {
        return new RecipeData
        {
            ID = recipe.ID,
            Image = recipe.RecipeImage.ImageURL,
            IngedientsCount = recipe.Ingredients.Count(),
            Name = recipe.RecipeName,
            Rating = recipe.Rating,
            TimeToCook = recipe.TimeToCook,
            Description = recipe.RecipeDescription,
            Ingredients = recipe.Ingredients.Select(x => new IngredientAmountData
            {
                ID = x.ID,
                Name = x.IngredientName,
                Amount = x.AmountsForRecipes.Single(e => e.RecipeID == recipe.ID).Amount
            }).ToList()
        };
    }
}

public enum RecipeDTOTypes
{
    Shortened,
    Preview,
    Full
}