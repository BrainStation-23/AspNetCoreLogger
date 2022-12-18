using Dapper;
using MassTransit.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using WebApp.Common.Serialize;
using WebApp.Logger.Extensions;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Loggers.Repositories
{
    public class SqlFileLogRepository : ISqlLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<SqlLogRepository> _logger;
        private readonly LogOption _logOption;

        public SqlFileLogRepository(DapperContext dapper,
            ILogger<SqlLogRepository> logger,
            IOptions<LogOption> logOption)
        {
            _dapper = dapper;
            _logger = logger;
            _logOption = logOption.Value;
        }

        public async Task AddAsync(SqlModel sqlModel)
        {
            if (sqlModel.Url.Contains("/Log/"))
                return;

            var fileConfig = _logOption.Provider.File;

            try
            {
                FileExtension.LogWrite(fileConfig.Path, null, sqlModel);
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(ExceptionLogRepository), exception);
            }
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            dynamic sqlLogs;
            var query = @"SELECT * FROM [dbo].[SqlLogs]
                            ORDER BY [Id] DESC
                            OFFSET @Offset ROWS 
                            FETCH NEXT  @Next   ROWS ONLY";

            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    var sqlLogsEntities = await connection.QueryAsync(query, pager);
                    var sqlLogUnescapeString =JsonSerializeExtentions.ToJson(sqlLogsEntities).JsonUnescaping();
                    sqlLogs = JArray.Parse(sqlLogUnescapeString);
                }

                return sqlLogs;
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(SqlLogRepository), exception);
                throw;
            }
        }
    }
}
