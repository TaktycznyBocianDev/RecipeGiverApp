﻿using Dapper;
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
        public async Task<ActionResult> GetRecipeWithIngredients(string recipeName)
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

        //[HttpPost("CreateIngredients")]
        //public async Task<ActionResult> CreateIngredientAsync(Ingredient ingredient)
        //{
        //    try
        //    {
        //        var result = await _ingredientService.CreateIngredientAsync(ingredient);
        //        if (result == 0) return NotFound("No ingredient created. Does it not exist already?");
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while creating the ingredient");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}

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