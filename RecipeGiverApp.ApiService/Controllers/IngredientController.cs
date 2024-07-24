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
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;
        private readonly ILogger<IngredientController> _logger;

        public IngredientController(IIngredientService ingredientService, ILogger<IngredientController> logger)
        {
            _ingredientService = ingredientService;
            _logger = logger;
        }

        [HttpGet("GetIngredients")]
        public async Task<ActionResult<List<Ingredient>>> GetIngredientsAsync(int? IngredientID = null, string? IngredientName = null)
        {
            try
            {
                var ingredients = await _ingredientService.GetIngredientsAsync(IngredientID, IngredientName);
                if (ingredients == null || ingredients.Count == 0) return NotFound("No categories found.");
                return Ok(ingredients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting ingredients");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

