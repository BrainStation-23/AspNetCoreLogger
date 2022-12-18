using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Common.Serialize;
using WebApp.Logger.Providers.Sqls;
using WebApp.Logger.Models;
using WebApp.Logger.Extensions;
using Microsoft.Extensions.Options;

namespace WebApp.Logger.Loggers.Repositories
{
    public class ExceptionFileLogRepository : IExceptionLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<ExceptionLogRepository> _logger;
        private readonly LogOption _logOption;

        public ExceptionFileLogRepository(DapperContext dapper,
            ILogger<ExceptionLogRepository> logger,
            IOptions<LogOption> logOption)
        {
            _dapper = dapper;
            _logger = logger;
            _logOption = logOption.Value;
        }

        public async Task AddAsync(ErrorModel errorModel)
        {
            if (errorModel.Url.Contains("/Log/"))
                return;

            var fileConfig = _logOption.Provider.File;

            try
            {
                errorModel = errorModel.PrepareErrorModel(_logOption);
                FileExtension.LogWrite(fileConfig.Path, null, errorModel);
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(ExceptionLogRepository), exception);
            }
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            dynamic logs;

            var query = @"SELECT * FROM [dbo].[ExceptionLogs]
                            ORDER BY [Id] DESC
                            OFFSET @Offset ROWS 
                            FETCH NEXT @Next ROWS ONLY";

            var exceptionLogUnescapeString = string.Empty;
            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    var exceptionLogs = await connection.QueryAsync<ExceptionLogVm>(query, pager);

                    exceptionLogs.ToList().ForEach(f =>
                    {
                        f.StackTrace = JsonConvert.SerializeObject(f.StackTrace);
                    });
                    exceptionLogUnescapeString = JsonSerializeExtentions.ToJson(exceptionLogs);
                    var unescape = exceptionLogUnescapeString.JsonUnescaping();
                    logs = JArray.Parse(unescape);
                }

                return logs;
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(ExceptionLogRepository), exception);
                throw;
            }
        }
    }
}
