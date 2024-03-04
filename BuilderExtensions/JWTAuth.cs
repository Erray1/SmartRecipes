using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SmartRecipes.BuilderExtensions;

public static class JWTAuthBuilderExtensions
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
}
