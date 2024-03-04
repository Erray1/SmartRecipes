using SmartRecipes.DataContext.Extensions;
using SmartRecipes.BuilderExtensions;
using SmartRecipes.DataContext.Repos.Extensions;
using SmartRecipes.Components;
using Microsoft.FeatureManagement;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRecipesContext(builder.Configuration);
builder.Services.AddUsersContext(builder.Configuration);
builder.Services.AddRepositories();

builder.Services.AddCustomServices()
    .AddJWTAuthentificationAndAuthorization(builder.Configuration)
    .AddCustomFeatures(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
