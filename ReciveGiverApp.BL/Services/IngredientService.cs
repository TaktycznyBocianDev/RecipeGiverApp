using ReciveGiverApp.Models.Models;
using Microsoft.AspNetCore.Mvc;
using ReciveGiverApp.Database.Data;
using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;

namespace ReciveGiverApp.BL.Services
{

    public interface IIngredientService
    {
        public Task<List<Ingredient>> GetIngredientsAsync(int? Id = null, string? Name = null);
        public Task<int> CreateIngredientAsync(Ingredient ingredient);

    }

    public class IngredientService : IIngredientService
    {

        private readonly ConnectionManager _connectionManager;
        private readonly ILogger<IngredientService> _logger;

        public IngredientService(ConnectionManager connectionManager, ILogger<IngredientService> logger)
        {
            _connectionManager = connectionManager;
            _logger = logger;
        }

        public async Task<List<Ingredient>> GetIngredientsAsync(int? IngredientID = null, string? IngredientName = null)
        {
            try
            {
                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open(); // Open the connection asynchronously

                    string sql = "SELECT * FROM Ingredients";
                    string param = "";

                    if (IngredientID != null) { param += " AND IngredientID = @IngredientID"; }
                    if (IngredientName != null) { param += " AND IngredientName = @IngredientName"; }

                    if (param.StartsWith(" AND"))
                    {
                        param = param.Substring(4);
                        param = " WHERE" + param;
                    }

                    sql = sql + param;

                    // Use Dapper to execute the query and get the result as a string
                    var result = await connection.QueryAsync<Ingredient>(sql, new { IngredientID = IngredientID, IngredientName = IngredientName });
                    var categories = result.ToList();
                    connection.Close();
                    return categories;
                    
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting categories");
                return new List<Ingredient>();
            }
        }

        public async Task<int> CreateIngredientAsync(Ingredient ingredient)
        {
            try
            {
                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open();

                    string sql = " INSERT INTO Ingredients (IngredientName) SELECT @IngredientName WHERE NOT EXISTS (SELECT 1 FROM Ingredients WHERE IngredientName = @IngredientName);";

                    var result = await connection.ExecuteAsync(sql, new { IngredientName = ingredient.IngredientName });
                    connection.Close();
                    return result;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return 0;
            }
        }

    }
}
