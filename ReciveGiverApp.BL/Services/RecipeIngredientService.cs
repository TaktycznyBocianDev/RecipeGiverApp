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
using System.Xml.Linq;

namespace ReciveGiverApp.BL.Services
{

    public interface IRecipeIngredientService
    {
        public Task<List<Recipe>> GetRecipesWithIngredientsAsync(string? Name = null);
        public Task<int> CreateRecipesWithCorespondingIngredientsAsync(Recipe recipe);
        public Task<int> UpdateIngredientsForRecipeAsync(string recipeName, Ingredient[] ingredients);
        public Task<int> DeleteRecipeAsync(string Name);
        public Task<int> DeleteIngredientFromRecipeRelationAsync(string recipeName, string ingridientName);
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
                        var result = await connection.QueryAsync<RecipeIngredientDTO>(sql, new { RecipeID = recipe.RecipeID });
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

        public async Task<int> CreateRecipesWithCorespondingIngredientsAsync(Recipe recipe)
        {
            try
            {
                //Create recipe
                int recipeCreated = await _recipeService.CreateRecipesNoIngredientsAsync(recipe);
                if (recipeCreated == 0) throw new Exception("No recipe was created");

                //Get recipe back with proper ID (it will be use soon)
                var reGetRecipeWithId = await _recipeService.GetRecipesAsync(recipe.RecipeName);
                Recipe recipeWithId = reGetRecipeWithId[0];

                //Create usefull variables
                int howManyIngredients = 0;
                string sql = "INSERT INTO RecipeIngredients (RecipeID, IngredientId, Quantity) VALUES (@RecipeID, @IngredientId, @Quantity)";
                List<Ingredient> finalIngredients = new List<Ingredient>();

                //Add every ingredient from provided recipe and get it back with proper ID - add it to list finalIngredients 
                foreach (var ing in recipe.ingredients)
                {
                    var addingIngredientsResult = await _ingredientService.CreateIngredientAsync(ing);
                    if (addingIngredientsResult == 0) throw new Exception("Failed to create ingredients");

                    var tmp = await _ingredientService.GetIngredientsAsync(null, ing.IngredientName);

                    Ingredient ingridinetWithId = tmp[0];
                    finalIngredients.Add(ingridinetWithId);
                }

                //Add every ingredient it's relation to recipe
                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open();
                    foreach (var ing in finalIngredients)
                    {
                        var result = await connection.ExecuteAsync(sql, new { RecipeID = recipeWithId.RecipeID, IngredientId = ing.IngredientID, Quantity = ing.Quantity });
                        if (result != 0) howManyIngredients += result;
                    }
                    connection.Close();
                }

                //Return how many ingredients were added
                return howManyIngredients;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return 0;
            }
        }

        public async Task<int> DeleteIngredientFromRecipeRelationAsync(string recipeName, string ingridientName)
        {
            try
            {
                var ing = await _ingredientService.GetIngredientsAsync(null, ingridientName);
                if (ing == null) throw new Exception($"No ingredient like this was found.");

                var recip = await _recipeService.GetRecipesAsync(recipeName);
                if (recip == null) throw new Exception("No recipe like this was found.");

                Ingredient ingredient = ing[0];
                Recipe recipe = recip[0];

                string sql = "DELETE FROM RecipeIngredients WHERE (RecipeID = @RecipeID) AND (IngredientId = @IngredientId)";

                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open();

                    var result = await connection.ExecuteAsync(sql, new { RecipeID = recipe.RecipeID, IngredientId = ingredient.IngredientID });

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




        public async Task<int> DeleteRecipeAsync(string Name)
        {
            //As removing recipe removes any recipes connected with it.
            return await _recipeService.DeleteRecipeAsync(Name);
        }



        public async Task<int> UpdateIngredientsForRecipeAsync(string recipeName, Ingredient[] ingredients)
        {
            
        }


    }
}
