using Microsoft.FeatureManagement;

namespace SmartRecipes.BuilderExtensions;


public static partial class BuilderExtensions
{
    public static IServiceCollection AddCustomFeatures(this IServiceCollection services, IConfiguration config)
    {
        services.AddFeatureManagement(config.GetSection("FeatureManagement"));
        return services;
    }
}
