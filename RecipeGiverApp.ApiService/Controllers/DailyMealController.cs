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
    public class DailyMealController : ControllerBase
    {
        private readonly IDailyMealsService _dailyMealsService;
        private readonly ILogger<CategoryController> _logger;

        public DailyMealController(IDailyMealsService dailyMealsService, ILogger<CategoryController> logger)
        {
            _dailyMealsService = dailyMealsService;
            _logger = logger;
        }

        [HttpGet("GetDayByDate")]
        public async Task<ActionResult<List<Category>>> GetDayByDateAsync(DateTime date)
        {
            try
            {
                var dailyMeal = await _dailyMealsService.GetDailyMealsAsync(date);
                if (dailyMeal == null || dailyMeal.Count == 0) return NotFound("No meals find for this day");
                return Ok(dailyMeal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting meals for this day");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetDayWithinTimeInterval")]
        public async Task<ActionResult<List<Category>>> GetDayWithinTimeIntervalAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var dailyMeal = await _dailyMealsService.GetDailyMealsAsync(startDate, endDate);
                if (dailyMeal == null || dailyMeal.Count == 0) return NotFound("No meals find for this time interval");
                return Ok(dailyMeal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting meals for this time interval");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("CreateSingleMealForTheDay")]
        public async Task<ActionResult<List<Category>>> CreateSingleMealForTheDay(Recipe recipe, DateTime date)
        {
            try
            {
                var dailyMeal = await _dailyMealsService.AddRecipeToDayAsync(recipe, date);
                if (dailyMeal == 0) return NotFound("No meals was created for day given!");
                return Ok(dailyMeal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating meal for this day!");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("CreateAllMealsForTheDay")]
        public async Task<ActionResult<List<Category>>> CreateAllMealsForTheDay(Recipe[] recipes, DateTime date)
        {
            try
            {
                int createdMeals = 0;

                foreach (var recipe in recipes)
                {
                    var dailyMeal = await _dailyMealsService.AddRecipeToDayAsync(recipe, date);
                    if (dailyMeal == 0) break;
                    createdMeals++;
                }

                if (createdMeals == 0) return NotFound("No meals was created for day given!");
                if (createdMeals < recipes.Length) return NotFound("No all meals were set to this day!");
                return Ok(createdMeals);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating meal for this day!");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("UpdateMealForTheDay")]
        public async Task<ActionResult<List<Category>>> UpdateMealForTheDay(Recipe recipe, DateTime date)
        {
            try
            {
                var dailyMeal = await _dailyMealsService.UpdateRecipeToDayForCategoryAsync(recipe, date);
                if (dailyMeal == 0) return NotFound("No meals was updated for that day in this category!");
                return Ok(dailyMeal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updated meal for this day in this category!");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DeleteRecipeFromDayAsync")]
        public async Task<ActionResult<List<Category>>> DeleteRecipeFromDayAsync(Recipe recipe, DateTime date)
        {
            try
            {
                var dailyMeal = await _dailyMealsService.DeleteRecipeFromDayAsync(recipe, date);
                if (dailyMeal == 0) return NotFound("No meals was deleted from that day in this category!");
                return Ok(dailyMeal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting meal for this day in this category!");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DeleteTheDay")]
        public async Task<ActionResult<List<Category>>> DeleteMealForTheDay(DateTime date)
        {
            try
            {
                var dailyMeal = await _dailyMealsService.DeleteDayAsync(date);
                if (dailyMeal == 0) return NotFound("No day was deleted!");
                return Ok(dailyMeal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting this day!");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}

