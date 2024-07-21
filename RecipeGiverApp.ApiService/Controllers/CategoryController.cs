using Dapper;
using Microsoft.AspNetCore.Mvc;
using ReciveGiverApp.Database.Data;
using ReciveGiverApp.Models.Models;
using System;
using System.Data;
using System.Threading.Tasks;

namespace RecipeGiverApp.ApiService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ConnectionManager _connectionManager;

        public CategoryController(ConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        [HttpGet("GetCategory")]
        public async Task<ActionResult<Category[]>> GetCategoryAsync(int? Id = null, string? Name = null)
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
                    Category[] categories = result.ToArray();

                    return Ok(categories);
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}" );
            }
        }
    }
}


//using Dapper;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Data.Sqlite;
//using ReciveGiverApp.Models.Models;

//namespace RecipeGiverApp.ApiService.Controllers
//{
//    [Route("[controller]")]
//    [ApiController]
//    public class CategoryController : Controller
//    {
//        private readonly SqliteConnection _connection;

//        public CategoryController(SqliteConnection connection)
//        {
//            _connection = connection;
//        }

//        [HttpGet("GetCategory")]
//        public async Task<Category[]> GetCategoryAsync(int? Id = null, string? Name = null)
//        {
//            try
//            {
//                await _connection.OpenAsync(); // Open the connection asynchronously

//                string sql = "SELECT * FROM Categories";
//                string param = "";

//                if (Name != null) { param += " AND CategoryName = @Name"; }
//                if (Id != null) { param += " AND CategoryID = @Id"; }

//                if (param.StartsWith(" AND"))
//                {
//                    param = param.Substring(4);
//                    param = " WHERE" + param;
//                }

//                sql = sql + param;

//                // Use Dapper to execute the query and get the result as a string
//                var result = await _connection.QueryAsync<Category>(sql, new { Name = Name, Id = Id });
//                Category[] categories = result.ToArray();

//                return categories;
//            }
//            catch (Exception ex)
//            {
//                // Log the exception or handle it as needed
//                Console.WriteLine($"An error occurred: {ex.Message}");
//                return null;
//            }
//            finally
//            {
//                await _connection.CloseAsync(); // Ensure the connection is closed
//            }

//        }
//    }
//}