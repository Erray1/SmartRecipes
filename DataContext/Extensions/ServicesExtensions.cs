using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartRecipes.DataContext.Recipes;
using SmartRecipes.DataContext.Users;
using SmartRecipes.DataContext.Users.Models;

namespace SmartRecipes.DataContext.Extensions;

public static partial class ServicesExtensions
{
    public static IServiceCollection AddRecipesContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RecipesContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("WebApiPostgreSQLDatabase"));
            options.LogTo(Console.WriteLine, new[] { Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting });
        });
        return services;
    }
    public static IServiceCollection AddUsersContext(this IServiceCollection services, IConfiguration configuration)
    {
        
        services.AddDbContext<UsersContext>(options => {
            options.UseNpgsql(configuration.GetConnectionString("WebApiPostgreSQLDatabase"), providerOptions =>
            {
                providerOptions.SetPostgresVersion(new Version(""));
            });
            options.LogTo(Console.WriteLine, new[] { Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting });
        });
        services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<UsersContext>();
        return services;
    }
}
