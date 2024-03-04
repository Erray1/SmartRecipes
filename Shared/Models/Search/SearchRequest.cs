using System.ComponentModel.DataAnnotations;

namespace SmartRecipes.Shared.Models.Search;

public sealed class SearchRequest
{
    [StringLength(75, MinimumLength = 2)]
    public string SearchString { get; set; }
}
