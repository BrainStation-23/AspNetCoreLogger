﻿using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebApp.Logger.Providers.Sqls
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly string _connectionStringName;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("WebAppConnection");
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