using System.Data;
using Microsoft.Data.Sqlite;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace ReciveGiverApp.Database.Data
{
    public  class ConnectionManager
    {
        private readonly IConfiguration _configuration;

        public ConnectionManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            Console.WriteLine("Current DefaultConnection: " + _configuration.GetConnectionString("DefaultConnection"));
            return new SqliteConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
