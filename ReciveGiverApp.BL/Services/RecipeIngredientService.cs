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

    public interface IRecipeIngredientService
    {
        public Task<List<Recipe>> GetRecipesWithIngredientsAsync(string? Name = null);
        public Task<List<Recipe>> GetRecipesWithIngredientsAsync(int Id);
        public Task<int> CreateRecipesWithCorespondingIngredientsAsync(Recipe recipe);
        public Task<int> UpdateQuantityAsync(string recipeName, string IngredientName, string quantity);
        public Task<int> DeleteRecipeAsync(string Name);
        public Task<int> DeleteIngredientFromRecipeRelationAsync(string recipeName, string ingridientName);

    }
    public class RecipeIngredientService : IRecipeIngredientService
    {
        private readonly ConnectionManager _connectionManager;
        private readonly IIngredientService _ingredientService;
        private readonly IRecipeService _recipeService;
        private readonly ILogger<RecipeIngredientService> _logger;
        private class Quantities
        {
            public int IngredientId { get; set; }
            public string Quantity { get; set; } = string.Empty;
            public Quantities(int ingredientId, string quantity)
            {
                this.IngredientId = ingredientId;
                Quantity = quantity;
            }
        }

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
                        recipe.Ingredients = ingredients;
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

        public async Task<List<Recipe>> GetRecipesWithIngredientsAsync(int Id)
        {

            try
            {
                var recipes = _recipeService.GetRecipeByIdAsync(Id).GetAwaiter().GetResult();
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
                        recipe.Ingredients = ingredients;
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
                //Create recipe and get it's ID
                int recipeCreated = await _recipeService.CreateRecipesNoIngredientsAsync(recipe);
                if (recipeCreated == 0) throw new Exception("No recipe was created");
                int recipeId = await _recipeService.GetRecipeIdByName(recipe.RecipeName);

                //Create usefull variables
                int howManyIngredients = 0;

                List<Quantities> finalIngredientsWithQuantities = new List<Quantities>();

                //Add every ingredient from provided recipe and get its' ID - add it to list finalIngredientsWithQuantities 
                foreach (var ing in recipe.Ingredients)
                {
                    var addingIngredientsResult = await _ingredientService.CreateIngredientAsync(ing);

                    int ingredientId = _ingredientService.GetIngredientIdByName(ing.IngredientName).Result;

                    Quantities quanti = new Quantities(ingredientId, ing.Quantity);

                    finalIngredientsWithQuantities.Add(quanti);
                }

                //Add every ingredient it's relation to recipe
                string sql = "INSERT INTO RecipeIngredients (RecipeID, IngredientId, Quantity) VALUES (@RecipeID, @IngredientId, @Quantity)";
                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open();
                    foreach (var quantities in finalIngredientsWithQuantities)
                    {
                        var result = await connection.ExecuteAsync(sql, new { RecipeID = recipeId, IngredientId = quantities.IngredientId, Quantity = quantities.Quantity });
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
        public async Task<int> UpdateQuantityAsync(string recipeName, string ingredientName, string quantity)
        {
            try
            {
                var recip = await _recipeService.GetRecipeIdByName(recipeName);
                if (recip == 0) throw new Exception("No recipe with this name was found");
                
                var ing = await _ingredientService.GetIngredientIdByName(ingredientName);
                if (ing == 0) throw new Exception("No ingredient with this name was found");

                int recipeID = recip;
                int ingredientID = ing;

                string sql = "UPDATE RecipeIngredients SET Quantity = @Quantity WHERE RecipeID = @RecipeID AND IngredientID = @IngredientID;";

                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open();

                    var result = await connection.ExecuteAsync(sql, new { Quantity = quantity, RecipeID = recipeID, IngredientID = ingredientID });

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

    }
}
