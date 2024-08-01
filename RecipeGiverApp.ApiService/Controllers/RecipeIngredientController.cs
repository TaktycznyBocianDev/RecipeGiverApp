using Dapper;
using Microsoft.AspNetCore.Mvc;
using ReciveGiverApp.BL.Services;
using ReciveGiverApp.Database.Data;
using ReciveGiverApp.Models.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace RecipeGiverApp.ApiService.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class RecipeIngredientController : ControllerBase
    {
        private readonly IRecipeIngredientService _recipeIngredientService;
        private readonly ILogger<IngredientController> _logger;

        public RecipeIngredientController(IRecipeIngredientService recipeIngredientService, ILogger<IngredientController> logger)
        {
            _recipeIngredientService = recipeIngredientService;
            _logger = logger;
        }

        [HttpGet("GetRecipeWithIngredients/{recipeName}")]
        public async Task<ActionResult> GetRecipeWithIngredientsAsync(string recipeName)
        {
            try
            {
                var recipe = await _recipeIngredientService.GetRecipesWithIngredientsAsync(recipeName);
                if (recipe == null || recipe.Count == 0) return NotFound("No recipe found.");
                 return Ok(recipe[0]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting recipe");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("GetRecipesWithIngredients")]
        public async Task<ActionResult<List<Ingredient>>> GetRecipesWithIngredientsAsync()
        {
            try
            {
                var recipes = await _recipeIngredientService.GetRecipesWithIngredientsAsync();
                if (recipes == null || recipes.Count == 0) return NotFound("No recipes found.");
                return Ok(recipes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting recipes");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("CreateRecipeWithIngredients")]
        public async Task<ActionResult> CreateRecipeWithIngredients(Recipe recipe)
        { 
            try
            {
                var result =  await _recipeIngredientService.CreateRecipesWithCorespondingIngredientsAsync(recipe);
                if (result == 0) return NotFound("No recipe created.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the recipe");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DeleteIngredientFromRelationWithRecipeAsync/{recipeName, ingridientName}")]
        public async Task<ActionResult> DeleteIngredientFromRecipeRelationAsync(string recipeName, string ingridientName)
        {
            try
            {
                var result = await _recipeIngredientService.DeleteIngredientFromRecipeRelationAsync(recipeName, ingridientName);
                if (result == 0) return NotFound("No ingredient deleted. Is name valid?");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the ingredient");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DeleteRecipe")]
        public async Task<ActionResult> DeleteRecipe(string recipeName)
        {
            try
            {
                var result = await _recipeIngredientService.DeleteRecipeAsync(recipeName);
                if (result == 0) return NotFound("No recipe deleted. Is name valid?");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing the recipe");
                return StatusCode(500, "Internal server error");
            }
        }


        //[HttpPut("UpdateIngredient")]
        //public async Task<ActionResult> UpdateIngredientNameAsync(int ingredientId, string newName)
        //{
        //    try
        //    {
        //        var result = await _ingredientService.UpdateIngredientNameAsync(ingredientId, newName);
        //        if (result == 0) return NotFound("No ingredient updated. Are Id and New Name valid?");
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while updating the ingredient");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

        //[HttpDelete("DeleteIngredient")]
        //public async Task<ActionResult> DeleteIngredientAsync(string ingredientName)
        //{
        //    try
        //    {
        //        var result = await _ingredientService.DeleteIngredientAsync(ingredientName);
        //        if (result == 0) return NotFound("No ingredient deleted. Is name valid?");
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while updating the ingredient");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}
    }
}