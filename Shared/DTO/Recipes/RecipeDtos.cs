using SmartRecipes.Shared.DTO;

namespace SmartRecipes.Shared.DTO.Recipes;



public class RecipeListDto<T> : DtoBase where T : RecipeDataBase
{
    public List<T> Content { get; set; }
}

public class RecipeDto<T> : DtoBase where T : RecipeDataBase
{
    public T Content { get; set; }
}


