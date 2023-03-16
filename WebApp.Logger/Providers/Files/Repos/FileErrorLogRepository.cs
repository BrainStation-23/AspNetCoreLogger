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

namespace WebApp.Logger.Providers.Files.Repos
{
    public class FileErrorLogRepository : IErrorLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<FileErrorLogRepository> _logger;
        private readonly LogOption _logOptions;

        public FileErrorLogRepository(DapperContext dapper,
            ILogger<FileErrorLogRepository> logger,
            IOptions<LogOption> logOption)
        {
            _dapper = dapper;
            _logger = logger;
            _logOptions = logOption.Value;
        }

        public async Task AddAsync(ErrorModel errorModel)
        {
            if (errorModel.Url.Contains("/Log/"))
                return;

            var fileConfig = _logOptions.Provider.File;

            try
            {
                errorModel = errorModel.DeserializeErrorModel().PrepareErrorModel(_logOptions);

                FileExtension.LogWrite(fileConfig, errorModel);
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(FileErrorLogRepository), exception);
            }

            await Task.CompletedTask;
        }

        public async Task AddAsync(List<ErrorModel> errorModels)
        {
            //if (errorModel.Url.Contains("/Log/"))
            //    return;

            var fileConfig = _logOptions.Provider.File;

            try
            {
                errorModels.ForEach(errorModel =>
                {
                    errorModel = errorModel.DeserializeErrorModel().PrepareErrorModel(_logOptions);
                });

                FileExtension.LogWrite(fileConfig, errorModels);
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(FileErrorLogRepository), exception);
            }

            await Task.CompletedTask;
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            var fileConfig = _logOptions.Provider.File;
            var exceptionLogs = fileConfig.Path.GetFilenames(LogType.Error.ToString());

            return await Task.FromResult(exceptionLogs);
        }

        public async Task RetentionAsync(DateTime dateTime)
        {
            dateTime.RetentionFileLogs(_logOptions.Provider.File.Path, LogType.Error.ToString());

            await Task.CompletedTask;
        }
    }
}
