using Microsoft.Extensions.Logging;
using ReciveGiverApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReciveGiverApp.BL.Services
{

    public interface IRecipeIngredientService
    {
        public Task<List<Recipe>> GetRecipesWithIngredientsAsync(string? Name = null, Recipe? recipe = null);
        public Task<int> CreateRecipesWithCorespondingIngredientsAsync(Recipe recipe, Ingredient[] ingredients);
        public Task<int> UpdateIngredientNameAsync(int ingredientId, string newName);
        public Task<int> DeleteIngredientAsync(string newName);
    }
    public class RecipeIngredientService : IRecipeIngredientService
    {
        private readonly IIngredientService _ingredientService;
        private readonly IRecipeService _recipeService;

        public RecipeIngredientService(IIngredientService ingredientService, IRecipeService recipeService)
        {
            _ingredientService = ingredientService;
            _recipeService = recipeService;
        }


        public Task<int> CreateRecipesWithCorespondingIngredientsAsync(Recipe recipe, Ingredient[] ingredients)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteIngredientAsync(string newName)
        {
            throw new NotImplementedException();
        }

        public Task<List<Recipe>> GetRecipesWithIngredientsAsync(string? Name = null, Recipe? recipe = null)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateIngredientNameAsync(int ingredientId, string newName)
        {
            throw new NotImplementedException();
        }
    }
}
