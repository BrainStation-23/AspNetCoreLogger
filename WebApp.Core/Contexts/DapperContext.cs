using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebApp.Core.Contexts
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly string _connectionStringName;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlConnection");
        }

        public DapperContext(IConfiguration configuration, string connectionStringName)
        {
            _configuration = configuration;
            _connectionStringName = connectionStringName;
            _connectionString = _configuration.GetConnectionString(_connectionStringName);
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}