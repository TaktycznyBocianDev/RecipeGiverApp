using Microsoft.AspNetCore.Mvc;
using ReciveGiverApp.BL.Services;
using ReciveGiverApp.Models.Models;

namespace RecipeGiverApp.ApiService.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {

        private readonly IRecipeService _recipeService;
        private readonly ILogger<CategoryController> _logger;

        public RecipeController(IRecipeService recipeService, ILogger<CategoryController> logger)
        {
            _recipeService = recipeService;
            _logger = logger;
        }

        [HttpGet("GetRecipes")]
        public async Task<ActionResult<List<Recipe>>> GetRecipesAsync(string? Name = null, int CategoryId = 0)
        {
            try
            {
                var recipes = await _recipeService.GetRecipesAsync(Name, CategoryId);
                if (recipes == null || recipes.Count == 0) return NotFound("No recipes found.");
                return Ok(recipes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting recipes");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GeIdByName")]
        public async Task<ActionResult> GeIdByName(string Name)
        {
            try
            {
                var recipes = await _recipeService.GetRecipeIdByName(Name);
                if (recipes == 0) return NotFound("No recipes with this name found.");
                return Ok(recipes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting recipe id");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("CreateRecipesNoIngredients")]
        public async Task<ActionResult> CreateRecipesNoIngredients(Recipe recipe)
        {
            try
            {
                var rowAffected = await _recipeService.CreateRecipesNoIngredientsAsync(recipe);
                if (rowAffected == 0) return NotFound("No recipes created.");
                return Ok(rowAffected);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating recipe");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("UpdateRecipeAsync")]
        public async Task<ActionResult> UpdateRecipeAsync(string OldName, Recipe recipeNew)
        {
            try
            {
                var rowAffected = await _recipeService.UpdateRecipeAsync(OldName, recipeNew);
                if (rowAffected == 0) return NotFound("No recipes updated.");
                return Ok(rowAffected);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating recipe");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DeleteRecipesAsync")]
        public async Task<ActionResult> DeleteRecipesAsync(string RecipeName)
        {
            try
            {
                var rowAffected = await _recipeService.DeleteRecipeAsync(RecipeName);
                if (rowAffected == 0) return NotFound("No recipes deleted.");
                return Ok(rowAffected);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting recipe");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
