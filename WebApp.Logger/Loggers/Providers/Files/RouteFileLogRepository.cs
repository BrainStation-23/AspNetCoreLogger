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
    public class RouteFileLogRepository : IRouteLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<RouteLogRepository> _logger;
        private readonly LogOption _logOption;

        public RouteFileLogRepository(DapperContext dapper,
            ILogger<RouteLogRepository> logger,
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
                _logger.LogError(nameof(RouteLogRepository), exception);
            }
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
                _logger.LogError(nameof(RouteLogRepository), exception);
            }
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            var fileConfig = _logOption.Provider.File;
            var routeLogs = FileExtension.GetFilenames(fileConfig.Path, LogType.Request.ToString());

            return routeLogs;
        }
        public async Task RetentionAsync(DateTime dateTime)
        {
            FileExtension.RetentionFileLogs(dateTime, _logOption.Provider.File.Path, LogType.Request.ToString());
        }
    }
}
