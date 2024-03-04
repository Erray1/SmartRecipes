using SmartRecipes.DataContext.Extensions;
using SmartRecipes.Services.Rating;
using SmartRecipes.Services.Recomendations;
using SmartRecipes.BuilderExtensions;
using SmartRecipes.Services.PathCalculator;
using SmartRecipes.Services.SearchEngines;
using SmartRecipes.DataContext.Repos.Extensions;
using SmartRecipes.DataContext.Repos.Filters.Shops.Filters;
using SmartRecipes.Components;


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

builder.Services.AddScoped<SimpleStrictSearch>();
builder.Services.AddScoped<SimpleLargeInputSearch>();

builder.Services.AddScoped<UserActionService>();

builder.Services.AddScoped<IShopsFilter, ShopsFilterV1>();
builder.Services.AddScoped<IPathFinder, RandomPathFinderService>();

builder.Services.AddScoped<RecomendationsService>();
builder.Services.AddScoped<RecomendationsMaker>();
builder.Services.AddScoped<SearchTokensWorker>();

builder.Services.AddJWTAuthentificationAndAuthorization(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseSwagger();
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
