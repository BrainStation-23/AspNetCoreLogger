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
    public class FileRequestLogRepository : IRequestLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<FileRequestLogRepository> _logger;
        private readonly LogOption _logOption;

        public FileRequestLogRepository(DapperContext dapper,
            ILogger<FileRequestLogRepository> logger,
            IOptions<LogOption> logOption)
        {
            _dapper = dapper;
            _logger = logger;
            _logOption = logOption.Value;
        }


        public async Task AddAsync(RequestModel requestModel)
        {
            if (requestModel.Url.Contains("/Log/"))
                return;

            var fileConfig = _logOption.Provider.File;

            try
            {
                requestModel = requestModel.DeserializeRequestModel().PrepareRequestModel(_logOption);
                FileExtension.LogWrite(fileConfig, requestModel);
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(FileRequestLogRepository), exception);
            }

            await Task.CompletedTask;
        }

        public async Task AddAsync(List<RequestModel> requestModels)
        {
            //if (requestModel.Url.Contains("/Log/"))
            //    return;

            var fileConfig = _logOption.Provider.File;

            try
            {
                requestModels.ForEach(requestModel =>
                {
                    requestModel = requestModel.DeserializeRequestModel().PrepareRequestModel(_logOption);
                });
                FileExtension.LogWrite(fileConfig, requestModels);
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(FileRequestLogRepository), exception);
            }

            await Task.CompletedTask;
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            var fileConfig = _logOption.Provider.File;
            var routeLogs = fileConfig.Path.GetFilenames(LogType.Request.ToString());

            return await Task.FromResult(routeLogs);
        }

        public async Task RetentionAsync(DateTime dateTime)
        {
            dateTime.RetentionFileLogs(_logOption.Provider.File.Path, LogType.Request.ToString());

            await Task.CompletedTask;
        }
    }
}
