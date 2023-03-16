using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Logger.Contracts;
using WebApp.Logger.Extensions;
using WebApp.Logger.Loggers;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Sqls;
using WebApp.Logger.Providers.Sqls.Repos;

namespace WebApp.Logger.Providers.Files.Repos
{
    public class FileSqlLogRepository : ISqlLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<FileSqlLogRepository> _logger;
        private readonly LogOption _logOptions;

        public FileSqlLogRepository(DapperContext dapper,
            ILogger<FileSqlLogRepository> logger,
            IOptions<LogOption> logOption)
        {
            _dapper = dapper;
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
                _logger.LogError(nameof(ErrorLogRepository), exception);
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
                _logger.LogError(nameof(ErrorLogRepository), exception);
            }

            await Task.CompletedTask;
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            var fileConfig = _logOptions.Provider.File;
            var sqlLogs = fileConfig.Path.GetFilenames(LogType.Sql.ToString());

            return await Task.FromResult(sqlLogs); ;
        }

        public async Task RetentionAsync(DateTime dateTime)
        {
            dateTime.RetentionFileLogs(_logOptions.Provider.File.Path, LogType.Sql.ToString());

            await Task.CompletedTask;
        }
    }
}
