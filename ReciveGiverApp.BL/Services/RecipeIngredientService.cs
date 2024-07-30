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
        public Task<Recipe> GetRecipesWithIngredientsAsync(string Name);
        public Task<List<Recipe>> GetRecipesWithIngredientsAsync();
        public Task<int> CreateRecipesWithCorespondingIngredientsAsync(Recipe recipe, Ingredient[] ingredients);
        public Task<int> UpdateIngredientNameAsync(int ingredientId, string newName);
        public Task<int> DeleteIngredientAsync(string newName);
    }
    public class RecipeIngredientService : IRecipeIngredientService
    {
        private readonly ConnectionManager _connectionManager;
        private readonly IIngredientService _ingredientService;
        private readonly IRecipeService _recipeService;
        private readonly ILogger _logger;

        public RecipeIngredientService(ConnectionManager connectionManager, IIngredientService ingredientService, IRecipeService recipeService, ILogger logger)
        {
            _connectionManager = connectionManager;
            _ingredientService = ingredientService;
            _recipeService = recipeService;
            _logger = logger;
        }

        public async Task<List<Recipe>> GetRecipesWithIngredientsAsync(string Name = "")
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
                        _recipeService.GetRecipesAsync();
                    }

                    connection.Close();
                   
                }
                return recipes;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return 0;
            }

           

            

            


            Recipe requestedRecipe = _recipeService.GetRecipesAsync(Name).GetAwaiter().GetResult()[0];
            if (requestedRecipe != null) throw new Exception($"There is no Recipe with name {Name}");


        }

        public Task<List<Recipe>> GetRecipesWithIngredientsAsync()
        {
            throw new NotImplementedException();
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
