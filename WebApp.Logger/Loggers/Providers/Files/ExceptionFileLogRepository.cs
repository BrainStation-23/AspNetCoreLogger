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
    public class ExceptionFileLogRepository : IErrorLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<ErrorLogRepository> _logger;
        private readonly LogOption _logOptions;

        public ExceptionFileLogRepository(DapperContext dapper,
            ILogger<ErrorLogRepository> logger,
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
                _logger.LogError(nameof(ErrorLogRepository), exception);
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
                _logger.LogError(nameof(ErrorLogRepository), exception);
            }

            await Task.CompletedTask;
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            var fileConfig = _logOptions.Provider.File;
            var exceptionLogs = FileExtension.GetFilenames(fileConfig.Path, LogType.Error.ToString());

            return await Task.FromResult(exceptionLogs);
        }

        public async Task RetentionAsync(DateTime dateTime)
        {
            FileExtension.RetentionFileLogs(dateTime, _logOptions.Provider.File.Path, LogType.Error.ToString());

            await Task.CompletedTask;
        }
    }
}
