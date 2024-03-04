using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRecipes.DataContext.Recipes.Models;

public sealed class Image : EntityModelBase
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string ID { get; set; }
    public Recipe RecipeWhereUsed { get; set; } = null!;
    public string ImageURL { get; set; }
    public Image()
    {
        
    }
}