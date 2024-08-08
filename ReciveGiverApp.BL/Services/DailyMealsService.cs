using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using ReciveGiverApp.Database.Data;
using ReciveGiverApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ReciveGiverApp.BL.Services
{
    public interface IDailyMealsService
    {

        public Task<List<DailyMeals>> GetDailyMealsAsync(DateTime date);
        public Task<List<DailyMeals>> GetDailyMealsAsync(DateTime startDate, DateTime endDate);
        public Task<int> AddRecipeToDayAsync(Recipe recipe, DateTime date);
        public Task<int> UpdateRecipeToDayForCategoryAsync(Recipe newRecipe, DateTime date);
        public Task<int> DeleteRecipeFromDayAsync(Recipe recipe, DateTime date);
        public Task<int> DeleteDayAsync(DateTime date);



    }
    public class DailyMealsService : IDailyMealsService
    {

        private readonly ConnectionManager _connectionManager;
        private readonly ILogger<CategoryService> _logger;

        public DailyMealsService(ConnectionManager connectionManager, ILogger<CategoryService> logger)
        {
            _connectionManager = connectionManager;
            _logger = logger;
        }

        public async Task<List<DailyMeals>> GetDailyMealsAsync(DateTime date)
        {
            try
            {
                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open();

                    string sql = "SELECT * FROM DailyMeals WHERE DayDate = @DayDate;";

                    var result = await connection.QueryAsync<DailyMeals>(sql, new { DayDate = FormatDate(date) });
                    var dailyMeals = result.ToList();
                    connection.Close();
                    return dailyMeals;

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting meals from {date}.");
                return new List<DailyMeals>();
            }
        }

        public async Task<List<DailyMeals>> GetDailyMealsAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open();

                    string sql = "SELECT * FROM DailyMeals WHERE DayDate BETWEEN @StartDate AND @EndDate;";
                    
                    var result = await connection.QueryAsync<DailyMeals>(sql, new { StartDate = FormatDate(startDate), EndDate = FormatDate(endDate) });
                    var dailyMeals = result.ToList();
                    connection.Close();
                    return dailyMeals;

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting meals from {startDate} to {endDate}");
                return new List<DailyMeals>();
            }
        }

        public async Task<int> AddRecipeToDayAsync(Recipe recipe, DateTime date)
        {
            try
            {
                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open();

                    string sql = "INSERT INTO DailyMeals(DayDate, CategoryID, RecipeID) VALUES (@DayDate, @CategoryID, @RecipeID)";

                    var result = await connection.ExecuteAsync(sql, new { DayDate = FormatDate(date), CategoryID = recipe.CategoryID, RecipeID = recipe.RecipeID });
                    connection.Close();
                    return result;

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while adding meal");
                return 0;
            }
        }

        public async Task<int> UpdateRecipeToDayForCategoryAsync(Recipe newRecipe, DateTime date)
        {
            try
            {
                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open();

                    string sql = "UPDATE DailyMeals SET RecipeID = @RecipeID  WHERE DayDate = @DayDate AND CategoryID = @CategoryID;";

                    var result = await connection.ExecuteAsync(sql, new { DayDate = FormatDate(date), CategoryID = newRecipe.CategoryID, RecipeID = newRecipe.RecipeID });
                    connection.Close();
                    return result;

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating meal");
                return 0;
            }
        }

        public async Task<int> DeleteDayAsync(DateTime date)
        {
            try
            {
                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open();

                    string sql = "DELETE FROM DailyMeals WHERE DayDate = @DayDate;";

                    var result = await connection.ExecuteAsync(sql, new { DayDate = FormatDate(date) });
                    connection.Close();
                    return result;

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting whole day");
                return 0;
            }
        }

        public async Task<int> DeleteRecipeFromDayAsync(Recipe recipe, DateTime date)
        {
            try
            {
                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open();

                    string sql = "DELETE FROM DailyMeals WHERE DayDate = @DayDate AND RecipeID = @RecipeID;";

                    var result = await connection.ExecuteAsync(sql, new { DayDate = FormatDate(date), RecipeID = recipe.RecipeID });
                    connection.Close();
                    return result;

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting meal from day");
                return 0;
            }
        }

       
        private string FormatDate (DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

    }
}
