using SmartRecipes.DataContext.Extensions;
using SmartRecipes.BuilderExtensions;
using SmartRecipes.DataContext.Repos.Extensions;
using Microsoft.AspNetCore.Identity;
using SmartRecipes.DataContext.Users.Models;
using SmartRecipes.DataContext.Users;
using SmartRecipes.Components;
using SmartRecipes.Application.Accounts;


WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddCascadingAuthenticationState(); //
builder.Services.AddAccountServices();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

builder.Services.AddRecipesContext(builder.Configuration);
builder.Services.AddUsersContext(builder.Configuration);

builder.Services.AddIdentityCore<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<UsersContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders(); // Test

builder.Services.AddRepositories();

builder.Services.AddCustomServices()
    .AddCustomFeatures(builder.Configuration);

builder.Services.AddResponseCaching();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseResponseCaching();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseAuthentication(); // ?
app.UseAuthorization();

app.MapAdditionalIdentityEndpoints();

//app.MapControllers();

app.Run();
