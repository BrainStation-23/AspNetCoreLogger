using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using WebApp.Common.Responses;
using WebApp.Core;
using WebApp.Logger.Extensions;
using WebApp.Logger.Loggers;
using WebApp.Logger.Loggers.Repositories;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger
{
    [ApiController]
    [Route("logger-wrapper")]
    public class LoggerWrapperController : ControllerBase
    {
        private readonly IRouteLogRepository _routeLogRepository;
        private readonly IExceptionLogRepository _exceptionLogRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly LogOption _logOption;
        private readonly ILog _loggerWrapper;

        public LoggerWrapperController(IRouteLogRepository routeLogRepository,
            IExceptionLogRepository exceptionLogRepository,
            IAuditLogRepository auditLogRepository,
            IServiceProvider serviceProvider,
            IOptions<LogOption> options)
        {
            _routeLogRepository = routeLogRepository;
            _exceptionLogRepository = exceptionLogRepository;
            _auditLogRepository = auditLogRepository;
            _serviceProvider = serviceProvider;
            _logOption = options.Value;

            var factory = new ProviderFactory(_serviceProvider);

            var providerType = _logOption.ProviderType;
            _loggerWrapper = factory.Build(providerType);
        }

        [HttpGet("routes")]
        public async Task<IActionResult> GetRouteLogsAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize, string searchText = null)
        {
            var res = await _loggerWrapper.Request.GetPageAsync(new DapperPager(pageIndex, pageSize));

            return new OkResponse(res);
        }

        [HttpGet("audits")]
        public async Task<IActionResult> GetAuditLogsAsync(int pageIndex = CommonVariables.pageIndex,
            int pageSize = CommonVariables.pageSize,
            string continuationToken = null,
            string searchText = null)
        {
            var res = await _loggerWrapper.Audit.GetPageAsync(new DapperPager(pageIndex, continuationToken, pageSize));

            return new OkResponse(res);
        }

        [HttpGet("exceptions")]
        public async Task<IActionResult> GetExceptionLogssAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize, string searchText = null)
        {
            var res = await _loggerWrapper.Error.GetPageAsync(new DapperPager(pageIndex, pageSize));

            return new OkResponse(res);
        }

        [HttpGet("sqls")]
        public async Task<IActionResult> GetSqlLogssAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize, string searchText = null)
        {
            var res = await _loggerWrapper.Sql.GetPageAsync(new DapperPager(pageIndex, pageSize));

            return new OkResponse(res);
        }

        [HttpGet("file/file_directories")]
        public async Task<IActionResult> GetFileLogsDirectoriesAsync()
        {
            var res = FileExtension.GetAllDirectories(_logOption.Provider.File);

            return new OkResponse(res);
        }

        [HttpGet("file/read_file")]
        public async Task<IActionResult> GetAllLogsFromSpecificFileAsync(string fileName)
        {
            var res = FileExtension.ParseLogFileToLogObject(_logOption.Provider.File,fileName);

            return new OkResponse(res);
        }

        [HttpGet("file/search_file")]
        public async Task<IActionResult> GetAllLogFilesBySearchKeyAsync(string searchKey)
        {
            var res = FileExtension.GetFileNamesBySearchKey(_logOption.Provider.File, searchKey);

            return new OkResponse(res);
        }
    }
}
