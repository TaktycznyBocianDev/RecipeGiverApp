using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReciveGiverApp.Database.Data;
using ReciveGiverApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReciveGiverApp.BL.Services
{

    public interface IRecipeIngredientService
    {
        public Task<List<Recipe>> GetRecipesWithIngredientsAsync(string? Name = null);
        public Task<int> CreateRecipesWithCorespondingIngredientsAsync(Recipe recipe, Ingredient[] ingredients);
        public Task<int> UpdateIngredientNameAsync(int ingredientId, string newName);
        public Task<int> DeleteIngredientAsync(string newName);
    }
    public class RecipeIngredientService : IRecipeIngredientService
    {
        private readonly ConnectionManager _connectionManager;
        private readonly IIngredientService _ingredientService;
        private readonly IRecipeService _recipeService;
        private readonly ILogger<RecipeIngredientService> _logger;

        public RecipeIngredientService(ConnectionManager connectionManager, IIngredientService ingredientService, IRecipeService recipeService, ILogger<RecipeIngredientService> logger)
        {
            _connectionManager = connectionManager;
            _ingredientService = ingredientService;
            _recipeService = recipeService;
            _logger = logger;
        }

        /// <summary>
        /// What will it do?
        /// </summary>
        public async Task<List<Recipe>> GetRecipesWithIngredientsAsync(string? Name = null)
        {

            try
            {
                var recipes = _recipeService.GetRecipesAsync(Name).GetAwaiter().GetResult();
                if (recipes == null) throw new Exception($"No recipes were found.");

                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open();

                    string sql = "SELECT * FROM RecipeIngredients WHERE RecipeID = @RecipeID";

                    foreach (var recipe in recipes)
                    {
                        //For each recipe, gets it's ingredients
                        var result = await connection.QueryAsync<RecipeIngredientDTO>(sql, new { RecipeID = recipe.RecipeID});     
                        List<RecipeIngredientDTO> recipeIngredients = result.ToList();

                        List<Ingredient> ingredients = new List<Ingredient>();

                        foreach (var item in recipeIngredients)
                        {

                            var nextIgredient = _ingredientService.GetIngredientsAsync(item.IngredientID).GetAwaiter().GetResult();
                            if (nextIgredient == null) break;

                            Ingredient ing = nextIgredient[0];
                            ingredients.Add(ing);
                        }
                        recipe.ingredients = ingredients;
                    }

                    connection.Close();
                   
                }
                return recipes;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new List<Recipe>();
            }
        }

        public Task<int> CreateRecipesWithCorespondingIngredientsAsync(Recipe recipe, Ingredient[] ingredients)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteIngredientAsync(string newName)
        {
            throw new NotImplementedException();
        }

        

        public Task<int> UpdateIngredientNameAsync(int ingredientId, string newName)
        {
            throw new NotImplementedException();
        }

        
    }
}
