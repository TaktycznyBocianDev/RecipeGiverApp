using Dapper;
using Microsoft.Extensions.Logging;
using ReciveGiverApp.Database.Data;
using ReciveGiverApp.Models.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReciveGiverApp.BL.Services
{
    public interface IRecipeService
    {

        public Task<List<Recipe>> GetRecipesAsync(string? Name = "", int CategoryId = 0);

    }
    public class RecipeService : IRecipeService
    {

        private readonly ConnectionManager _connectionManager;
        private readonly ILogger<CategoryService> _logger;

        public RecipeService(ConnectionManager connectionManager, ILogger<CategoryService> logger)
        {
            _connectionManager = connectionManager;
            _logger = logger;
        }

        public async Task<List<Recipe>> GetRecipesAsync(string? Name = null, int CategoryId = 0)
        {
            try
            {
                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open();
                    string sql = "SELECT * FROM Recipes";
                    string param = "";

                    if (Name != null) { param += " AND RecipeName = @Name"; }
                    if (CategoryId != 0) { param += " AND CategoryId = @CategoryId"; }

                    if (param.StartsWith(" AND"))
                    {
                        param = param.Substring(4);
                        param = " WHERE" + param;
                    }

                    sql = sql + param;

                    var result = await connection.QueryAsync<Recipe>(sql, new { Name = Name, CategoryId = CategoryId });
                    var recipes = result.ToList();

                    Console.WriteLine(sql);

                    connection.Close();
                    return recipes;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new List<Recipe>();
            }
        }
    }
}
