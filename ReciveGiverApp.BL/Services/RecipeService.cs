using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReciveGiverApp.Database.Data;
using ReciveGiverApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ReciveGiverApp.BL.Services
{
    public interface IRecipeService
    {
        public Task<int> GetRecipeIdByName(string RecipeName);
        public Task<List<Recipe>> GetRecipesAsync(string? Name = "", int CategoryId = 0);
        public Task<List<Recipe>> GetRecipeByIdAsync(int Id);
        public Task<int> CreateRecipesNoIngredientsAsync(Recipe recipe);
        public Task<int> UpdateRecipeAsync(string recipeOldName, Recipe recipeNew);
        public Task<int> DeleteRecipeAsync(string recipeName);
    }
    public class RecipeService : IRecipeService
    {

        private readonly ConnectionManager _connectionManager;
        private readonly ILogger<CategoryService> _logger;

        public RecipeService(ConnectionManager connectionManager, ILogger<CategoryService> logger)
        {
            _connectionManager = connectionManager;
            _logger = logger;
        }

        public async Task<int> GetRecipeIdByName(string recipeName)
        {
            try
            {
                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open();

                    string sql = "SELECT RecipeID FROM Recipes WHERE RecipeName = @RecipeName;";
                    var result = await connection.QueryFirstOrDefaultAsync<int>(sql, new { RecipeName = recipeName });

                    connection.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return 0;
            }
        }


        public async Task<List<Recipe>> GetRecipesAsync(string? Name = null, int CategoryId = 0)
        {
            try
            {
                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open();
                    string sql = "SELECT * FROM Recipes";
                    string param = "";

                    if (Name != null) { param += " AND RecipeName = @Name"; }
                    if (CategoryId != 0) { param += " AND CategoryId = @CategoryId"; }

                    if (param.StartsWith(" AND"))
                    {
                        param = param.Substring(4);
                        param = " WHERE" + param;
                    }

                    sql = sql + param;

                    var result = await connection.QueryAsync<Recipe>(sql, new { Name = Name, CategoryId = CategoryId });
                    var recipes = result.ToList();

                    Console.WriteLine(sql);

                    connection.Close();
                    return recipes;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new List<Recipe>();
            }
        }

        public async Task<List<Recipe>> GetRecipeByIdAsync(int Id)
        {
            try
            {
                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open();
                    string sql = "SELECT * FROM Recipes WHERE RecipeID = @RecipeID";

                    var result = await connection.QueryAsync<Recipe>(sql, new { RecipeID = Id});
                    var recipes = result.ToList();

                    Console.WriteLine(sql);

                    connection.Close();
                    return recipes;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new List<Recipe>();
            }
        }
        public async Task<int> CreateRecipesNoIngredientsAsync(Recipe recipe)
        {
            try
            {
                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open();

                    string sql = @"
                INSERT INTO Recipes (RecipeName, CategoryID, Instructions, FitPorada, Kilocalories)
                SELECT @RecipeName, @CategoryID, @Instructions, @FitPorada, @Kilocalories
                WHERE NOT EXISTS (SELECT 1 FROM Recipes WHERE RecipeName = @RecipeName);";

                    var result = await connection.ExecuteAsync(sql, new { RecipeName = recipe.RecipeName, CategoryID = recipe.CategoryID, Instructions = recipe.Instructions, FitPorada = recipe.FitPorada, Kilocalories = recipe.Kilocalories });
                    connection.Close();
                    return result;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return 0;
            }
        }

        public async Task<int> UpdateRecipeAsync(string recipeOldName, Recipe recipeNew)
        {
            try
            {
                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open();

                    string sql = @"
                UPDATE Recipes
                SET RecipeName = @RecipeName,
                    CategoryID = @CategoryID,
                    Instructions = @Instructions,
                    FitPorada = @FitPorada,
                    Kilocalories = @Kilocalories
                WHERE RecipeName = @RecipeOldName;";

                    var result = await connection.ExecuteAsync(sql, new
                    {
                        recipeNew.RecipeName,
                        recipeNew.CategoryID,
                        recipeNew.Instructions,
                        recipeNew.FitPorada,
                        recipeNew.Kilocalories,
                        RecipeOldName = recipeOldName
                    });
                    connection.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return 0;
            }
        }


        public async Task<int> DeleteRecipeAsync(string recipeName)
        {
            try
            {
                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open();

                    string sql = "DELETE FROM Recipes WHERE recipeName = @recipeName;";

                    var result = await connection.ExecuteAsync(sql, new { recipeName });
                    connection.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return 0;
            }
        }

    }
}
