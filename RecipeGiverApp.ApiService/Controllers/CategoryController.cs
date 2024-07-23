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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet("GetCategories")]
        public async Task<ActionResult<List<Category>>> GetCategoriesAsync(int? Id = null, string? Name = null)
        {
            try
            {
                var categories = await _categoryService.GetCategoriesAsync(Id, Name);
                if (categories == null || categories.Count == 0) return NotFound("No categories found.");
                return Ok(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting categories");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

