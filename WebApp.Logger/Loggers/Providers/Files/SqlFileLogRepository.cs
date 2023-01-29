using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Logger.Extensions;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Loggers.Repositories
{
    public class SqlFileLogRepository : ISqlLogRepository
    {
        private readonly ILogger<SqlLogRepository> _logger;
        private readonly LogOption _logOptions;

        public SqlFileLogRepository(ILogger<SqlLogRepository> logger,
            IOptions<LogOption> logOption)
        {
            _logger = logger;
            _logOptions = logOption.Value;
        }

        public async Task AddAsync(SqlModel sqlModel)
        {
            if (sqlModel.Url.Contains("/Log/"))
                return;

            var fileConfig = _logOptions.Provider.File;

            try
            {
                sqlModel = sqlModel.PrepareSqlModel(_logOptions);
                FileExtension.LogWrite(fileConfig, sqlModel);
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(ExceptionLogRepository), exception);
            }

            await Task.CompletedTask;
        }

        public async Task AddAsync(List<SqlModel> sqlModels)
        {
            //if (sqlModel.Url.Contains("/Log/"))
            //    return;

            var fileConfig = _logOptions.Provider.File;

            try
            {
                sqlModels.ForEach(sqlModel =>
                {
                    sqlModel = sqlModel.PrepareSqlModel(_logOptions);
                });
                FileExtension.LogWrite(fileConfig, sqlModels);
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(ExceptionLogRepository), exception);
            }

            await Task.CompletedTask;
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            var fileConfig = _logOptions.Provider.File;
            var sqlLogs = FileExtension.GetFilenames(fileConfig.Path, LogType.Sql.ToString());

            return await Task.Run(() => sqlLogs);
        }
        public async Task RetentionAsync(DateTime dateTime)
        {
            await FileExtension.RetentionFileLogs(dateTime, _logOptions.Provider.File.Path, LogType.Sql.ToString());
        }
    }
}
