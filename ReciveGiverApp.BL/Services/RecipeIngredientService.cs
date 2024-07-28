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
        public Task<int> CreateRecipesWithCorespondingIngredientsAsync(Ingredient ingredient);
        public Task<int> UpdateIngredientNameAsync(int ingredientId, string newName);
        public Task<int> DeleteIngredientAsync(string newName);
    }
    public class RecipeIngredientService : IRecipeIngredientService
    {
        public Task<int> CreateRecipesWithCorespondingIngredientsAsync(Ingredient ingredient)
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
