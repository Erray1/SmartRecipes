using System.ComponentModel.DataAnnotations.Schema;

namespace SmartRecipes.DataContext.Recipes.Models;

public sealed class Category : EntityModelBase
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string ID { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public ICollection<Recipe> RecipesWhereUsed { get; } = new List<Recipe>();
    public Category()
    {
        
    }
}
