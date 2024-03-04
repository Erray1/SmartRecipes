using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SmartRecipes.DataContext.Recipes.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace SmartRecipes.DataContext.Recipes;

public class RecipesContext : DbContext
{
    public RecipesContext() { }
    public RecipesContext(DbContextOptions<RecipesContext> options) : base(options) { }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Image> Images { get; set; }
    public virtual DbSet<Ingredient> Ingredients { get; set; }
    public virtual DbSet<Shop> Shops { get; set; }
    public virtual DbSet<Recipe> Recipes { get; set; }
    public virtual DbSet<IngredientPriceForShop> IngredientPrices { get; set; }
    public virtual DbSet<IngredientAmountForRecipe> IngredientAmounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        JsonSerializerOptions options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        // Categories

        modelBuilder.Entity<Category>()
            .HasKey(e => e.ID);

        modelBuilder.Entity<Category>()
            .HasMany(e => e.RecipesWhereUsed)
            .WithOne(e => e.Category);

        modelBuilder.Entity<Category>()
            .Property(e => e.CategoryName)
            .HasMaxLength(30);
         

        // Images

        modelBuilder.Entity<Image>()
            .HasKey(e => e.ID);
        

        // Ingredients

        modelBuilder.Entity<Ingredient>()
            .HasKey(e => e.ID);
        modelBuilder.Entity<Ingredient>()
            .Property(e => e.IngredientName)
            .HasMaxLength(30);

        // Shops

        modelBuilder.Entity<Shop>()
            .HasKey(e => e.ID);


        modelBuilder.Entity<Shop>()
            .HasMany(e => e.AvailableIngredients)
            .WithMany(e => e.ShopsWhereAvailable)
            .UsingEntity<IngredientPriceForShop>(
                l => l.HasOne<Ingredient>().WithMany(e => e.PriceInShops).HasForeignKey(e => e.ShopID),
                r => r.HasOne<Shop>().WithMany(e => e.IngredientPrices).HasForeignKey(e => e.IngredientID),
                j => j.Property(e => e.Price).HasDefaultValueSql("0")); // Проставить тип
        modelBuilder.Entity<Shop>()
            .Property(e => e.Name)
            .HasMaxLength(30)
            .IsRequired();
        modelBuilder.Entity<Shop>()
            .Property(e => e.Address)
            .HasMaxLength(100)
            .IsRequired();

        // Recipes

        modelBuilder.Entity<Recipe>()
            .HasKey(e => e.ID);


        modelBuilder.Entity<Recipe>()
            .HasMany(e => e.Ingredients)
            .WithMany(e => e.RecipesWhereUsed)
            .UsingEntity<IngredientAmountForRecipe>(
                l => l.HasOne(e => e.Ingredient).WithMany(e => e.AmountsForRecipes).HasForeignKey(e => e.RecipeID),
                r => r.HasOne(e => e.Recipe).WithMany(e => e.IngredientsAmounts).HasForeignKey(e => e.IngredientID),
                j => j.Property(e => e.Amount).HasDefaultValueSql("0 кг")
            );

        modelBuilder.Entity<Recipe>()
            .HasOne(e => e.RecipeImage)
            .WithOne(e => e.RecipeWhereUsed);

        modelBuilder.Entity<Recipe>()
            .Property(e => e.Rating)
            .HasConversion(
                v => JsonSerializer.Serialize(v, options),
                s => JsonSerializer.Deserialize<Dictionary<string, int>>(s, options)!,
                ValueComparer.CreateDefault(typeof(Dictionary<string, int>), false)
            )
            .IsRequired();
        modelBuilder.Entity<Recipe>()
            .Property(e => e.RecipeName)
            .HasMaxLength(30)
            .IsRequired();
        modelBuilder.Entity<Recipe>()
            .Property(e => e.RecipeDescription)
            .HasMaxLength(6400)
            .IsRequired();

    }
}
