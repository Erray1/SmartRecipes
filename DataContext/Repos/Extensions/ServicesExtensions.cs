using SmartRecipes.DataContext.Repos;
using SmartRecipes.DataContext.Extensions;

namespace SmartRecipes.DataContext.Repos.Extensions;

public static partial class ServicesExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IShopsRepository, ShopsRepository>();
        services.AddScoped<IRecipesRepository, RecipesRepository>();
        return services;
    }
}
