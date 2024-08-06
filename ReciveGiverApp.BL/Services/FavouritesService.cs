using Dapper;
using Microsoft.Extensions.Logging;
using ReciveGiverApp.Database.Data;
using ReciveGiverApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ReciveGiverApp.BL.Services
{

    public interface IFavouritesService
    {
        public Task<List<int>> GetFavourites();
        public Task<int> AddToFavourities(int recipeID);
        public Task<int> AddToFavourities(string recipeName);
        public Task<int> DeleteFavourites(int recipeID);
        public Task<int> DeleteFavourites(string recipeName);
    }

    public class FavouritesService : IFavouritesService
    {

        private readonly ConnectionManager _connectionManager;
        private readonly ILogger<CategoryService> _logger;

        public FavouritesService(ConnectionManager connectionManager, ILogger<CategoryService> logger)
        {
            _connectionManager = connectionManager;
            _logger = logger;
        }

        public async Task<int> AddToFavourities(int recipeID)
        {
            try
            {
                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open(); // Open the connection asynchronously

                    string sql = "INSERT INTO Favourites (RecipeID) VALUES (@RecipeID)";

                    var result = await connection.ExecuteAsync(sql, new { RecipeID  = recipeID});
                    connection.Close();
                    return result;

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding to favourities");
                return 0;
            }
        }

        public async Task<int> AddToFavourities(string recipeName)
        {
            try
            {
                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open(); // Open the connection asynchronously

                    string sql = "INSERT INTO Favourites (RecipeID) SELECT RecipeID FROM Recipes WHERE RecipeName = @RecipeName;";

                    var result = await connection.ExecuteAsync(sql, new { RecipeName = recipeName });
                    connection.Close();
                    return result;

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding to favourities");
                return 0;
            }
        }

        public async Task<int> DeleteFavourites(int recipeID)
        {
            try
            {
                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open(); 

                    string sql = "DELETE FROM Favourites WHERE RecipeID = @RecipeID";

                    var result = await connection.ExecuteAsync(sql, new { RecipeID = recipeID });
                    connection.Close();
                    return result;

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting favourites");
                return 0;
            }
        }

        public async Task<int> DeleteFavourites(string recipeName)
        {
            try
            {
                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open(); 

                    string sql = "DELETE FROM Favourites WHERE RecipeID = (SELECT RecipeID FROM Recipes WHERE RecipeName = @RecipeName);";

                    var result = await connection.ExecuteAsync(sql, new { RecipeName = recipeName });
                    connection.Close();
                    return result;

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting favourites");
                return 0;
            }
        }

        public async Task<List<int>> GetFavourites()
        {
            try
            {
                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open(); // Open the connection asynchronously

                    string sql = "SELECT RecipeID FROM Favourites;";

                    var result = await connection.QueryAsync<int>(sql);
                    var favouritesIDs = result.ToList();
                    connection.Close();
                    return favouritesIDs;

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting favourites");
                return new List<int>();
            }
        }
    }
}
