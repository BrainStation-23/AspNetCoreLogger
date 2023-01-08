using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using WebApp.Common.Serialize;
using WebApp.Logger.Providers.Sqls;
using WebApp.Logger.Models;
using Microsoft.Extensions.Options;
using WebApp.Logger.Extensions;

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
                requestModel = requestModel.PrepareRequestModel(_logOption);
                FileExtension.LogWrite(fileConfig, requestModel);
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
            
        }
    }
}
