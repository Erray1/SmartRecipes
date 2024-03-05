using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartRecipes.DataContext.Migrations.Recipes
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    CategoryName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    IngredientName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    RecipeName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CategoryID = table.Column<string>(type: "text", nullable: false),
                    RecipeDescription = table.Column<string>(type: "character varying(6400)", maxLength: 6400, nullable: false),
                    Rating = table.Column<string>(type: "text", nullable: false),
                    TimeToCook = table.Column<float>(type: "real", nullable: false),
                    TimesVisited = table.Column<int>(type: "integer", nullable: false),
                    TimesLiked = table.Column<int>(type: "integer", nullable: false),
                    TimesDisliked = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Recipes_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IngredientPrices",
                columns: table => new
                {
                    AvailableIngredientsID = table.Column<string>(type: "text", nullable: false),
                    ShopsWhereAvailableID = table.Column<string>(type: "text", nullable: false),
                    IngredientID = table.Column<string>(type: "text", nullable: true),
                    ShopID = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<float>(type: "real", nullable: false, defaultValueSql: "0")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientPrices", x => new { x.AvailableIngredientsID, x.ShopsWhereAvailableID });
                    table.ForeignKey(
                        name: "FK_IngredientPrices_Ingredients_AvailableIngredientsID",
                        column: x => x.AvailableIngredientsID,
                        principalTable: "Ingredients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IngredientPrices_Ingredients_IngredientID",
                        column: x => x.IngredientID,
                        principalTable: "Ingredients",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_IngredientPrices_Shops_ShopID",
                        column: x => x.ShopID,
                        principalTable: "Shops",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_IngredientPrices_Shops_ShopsWhereAvailableID",
                        column: x => x.ShopsWhereAvailableID,
                        principalTable: "Shops",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ID = table.Column<string>(type: "text", nullable: false),
                    ImageURL = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Images_Recipes_ID",
                        column: x => x.ID,
                        principalTable: "Recipes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IngredientAmounts",
                columns: table => new
                {
                    IngredientID = table.Column<string>(type: "text", nullable: false),
                    RecipeID = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientAmounts", x => new { x.IngredientID, x.RecipeID });
                    table.ForeignKey(
                        name: "FK_IngredientAmounts_Ingredients_RecipeID",
                        column: x => x.RecipeID,
                        principalTable: "Ingredients",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IngredientAmounts_Recipes_IngredientID",
                        column: x => x.IngredientID,
                        principalTable: "Recipes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IngredientAmounts_RecipeID",
                table: "IngredientAmounts",
                column: "RecipeID");

            migrationBuilder.CreateIndex(
                name: "IX_IngredientPrices_IngredientID",
                table: "IngredientPrices",
                column: "IngredientID");

            migrationBuilder.CreateIndex(
                name: "IX_IngredientPrices_ShopID",
                table: "IngredientPrices",
                column: "ShopID");

            migrationBuilder.CreateIndex(
                name: "IX_IngredientPrices_ShopsWhereAvailableID",
                table: "IngredientPrices",
                column: "ShopsWhereAvailableID");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CategoryID",
                table: "Recipes",
                column: "CategoryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "IngredientAmounts");

            migrationBuilder.DropTable(
                name: "IngredientPrices");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Ingredients");

            migrationBuilder.DropTable(
                name: "Shops");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
