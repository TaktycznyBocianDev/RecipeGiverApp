using Microsoft.AspNetCore.Mvc;
using ReciveGiverApp.BL.Services;
using ReciveGiverApp.Models.Models;

namespace RecipeGiverApp.ApiService.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class RecipeController :ControllerBase
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


    }
}
