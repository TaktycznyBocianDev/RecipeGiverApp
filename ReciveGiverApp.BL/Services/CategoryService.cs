using ReciveGiverApp.Models.Models;
using Microsoft.AspNetCore.Mvc;
using ReciveGiverApp.Database.Data;
using System.Data;
using Dapper;
using Microsoft.Extensions.Logging;

namespace ReciveGiverApp.BL.Services
{

    public interface ICategoryService
    {
        public Task<List<Category>> GetCategoriesAsync(int? Id = null, string? Name = null);
    }

    public class CategoryService : ICategoryService
    {

        private readonly ConnectionManager _connectionManager;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ConnectionManager connectionManager, ILogger<CategoryService> logger)
        {
            _connectionManager = connectionManager;
            _logger = logger;
        }

        public async Task<List<Category>> GetCategoriesAsync(int? Id = null, string? Name = null)
        {
            try
            {
                using (IDbConnection connection = _connectionManager.CreateConnection())
                {
                    connection.Open(); // Open the connection asynchronously

                    string sql = "SELECT * FROM Categories";
                    string param = "";

                    if (Name != null) { param += " AND CategoryName = @Name"; }
                    if (Id != null) { param += " AND CategoryID = @Id"; }

                    if (param.StartsWith(" AND"))
                    {
                        param = param.Substring(4);
                        param = " WHERE" + param;
                    }

                    sql = sql + param;

                    // Use Dapper to execute the query and get the result as a string
                    var result = await connection.QueryAsync<Category>(sql, new { Name = Name, Id = Id });
                    var categories = result.ToList();

                    return categories;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting categories");
                return new List<Category>();
            }
        }
    }
}
