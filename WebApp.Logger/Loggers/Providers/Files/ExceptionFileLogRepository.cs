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
        private readonly LogOption _logOptions;

        public ExceptionFileLogRepository(DapperContext dapper,
            ILogger<ExceptionLogRepository> logger,
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
                errorModel = errorModel.PrepareErrorModel(_logOptions);

                FileExtension.LogWrite(fileConfig, errorModel);
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(ExceptionLogRepository), exception);
            }
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            var fileConfig = _logOptions.Provider.File;
            var exceptionLogs = FileExtension.GetFilenames(fileConfig.Path,LogType.Error.ToString());

            return exceptionLogs;
        }
    }
}
