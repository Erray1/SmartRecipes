using Blazored.LocalStorage;
using Blazored.Modal;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SmartRecipes.Application.API;
using SmartRecipes.Application.Authorization;
using SmartRecipes.DataContext.Repos.Filters.Shops.Filters;
using SmartRecipes.Services.PathCalculator;
using SmartRecipes.Services.Rating;
using SmartRecipes.Services.Recomendations;
using SmartRecipes.Services.SearchEngines;
using System.Text;

namespace SmartRecipes.BuilderExtensions;

public static partial class BuilderExtensions
{
    public static IServiceCollection AddJWTAuthentificationAndAuthorization(this IServiceCollection services, IConfiguration config)
    {
        services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = config["JWTIssuer"],
                ValidAudience = config["JWTAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWTSecurityKey"]!))
            };
        });

        services.AddAuthorization(options =>
        {

        });

        return services;
    }
    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddScoped<SimpleStrictSearch>();
        services.AddScoped<SimpleLargeInputSearch>();

        services.AddScoped<UserActionService>();

        services.AddScoped<IShopsFilter, ShopsFilterV1>();
        services.AddScoped<IPathFinder, RandomPathFinderService>();

        services.AddScoped<RecomendationsService>();
        services.AddScoped<RecomendationsMaker>();
        services.AddScoped<SearchTokensWorker>();

        return services;
    }

    public static IServiceCollection AddClientSideServices(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddBlazoredModal();
        services.AddBlazoredLocalStorage();
        services.AddScoped<SmartRecipesAPIClient>();
        services.AddScoped<IAuthAPIClient, AuthAPIClient>();
        services.AddScoped<SmartRecipesAuthenticationStateProvider>();
        // services.AddCascadingAuthenticationState(); 
        return services;
    }
}
