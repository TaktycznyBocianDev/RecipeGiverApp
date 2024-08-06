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
    public class FavouritesController : ControllerBase
    {
        private readonly IFavouritesService _favouritiesService;
        private readonly ILogger<CategoryController> _logger;

        public FavouritesController(IFavouritesService favouritiesService, ILogger<CategoryController> logger)
        {
            _favouritiesService = favouritiesService;
            _logger = logger;
        }

        [HttpGet("GetFavourities")]
        public async Task<ActionResult<List<int>>> GetFavouritiesAsync()
        {
            try
            {
                var fav = await _favouritiesService.GetFavourites();
                if (fav == null || fav.Count == 0) return NotFound("No favourities found.");
                return Ok(fav);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting favourities");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("AddToFavouritiesById")]
        public async Task<ActionResult> AddToFavourities(int recipeID)
        {
            try
            {
                var fav = await _favouritiesService.AddToFavourities(recipeID);
                if (fav == 0) return NotFound("No favourities added.");
                return Ok(fav);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while added favourities");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("AddToFavouritiesByName")]
        public async Task<ActionResult> AddToFavourities(string recipeName)
        {
            try
            {
                var fav = await _favouritiesService.AddToFavourities(recipeName);
                if (fav == 0) return NotFound("No favourities added.");
                return Ok(fav);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while added favourities");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("RemoveFromFavouritiesById")]
        public async Task<ActionResult> RemoveFromFavourities(int recipeID)
        {
            try
            {
                var fav = await _favouritiesService.DeleteFavourites(recipeID);
                if (fav == 0) return NotFound("No favourities deleted.");
                return Ok(fav);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting favourities");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("RemoveFromFavouritiesByName")]
        public async Task<ActionResult> RemoveFromFavourities(string recipeName)
        {
            try
            {
                var fav = await _favouritiesService.DeleteFavourites(recipeName);
                if (fav == 0) return NotFound("No favourities deleted.");
                return Ok(fav);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting favourities");
                return StatusCode(500, "Internal server error");
            }
        }

    }
}

