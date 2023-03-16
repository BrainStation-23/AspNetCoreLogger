using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using WebApp.Logger.Contracts;
using WebApp.Logger.Defaults;
using WebApp.Logger.Extensions;
using WebApp.Logger.Loggers;
using WebApp.Logger.Providers.Sqls;
using WebApp.Logger.Responses;

namespace WebApp.Logger
{
    [ApiController]
    [Route("logger-wrapper")]
    public class LoggerWrapperController : ControllerBase
    {
        private readonly ISqlLogRepository _sqlLogRepository;
        private readonly IRequestLogRepository _routeLogRepository;
        private readonly IErrorLogRepository _exceptionLogRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly LogOption _logOption;

        public LoggerWrapperController(IRequestLogRepository routeLogRepository,
            IErrorLogRepository exceptionLogRepository,
            IAuditLogRepository auditLogRepository,
            ISqlLogRepository sqlLogRepository,
            IOptions<LogOption> logOption)
        {
            _routeLogRepository = routeLogRepository;
            _exceptionLogRepository = exceptionLogRepository;
            _auditLogRepository = auditLogRepository;
            _sqlLogRepository = sqlLogRepository;
            _logOption = logOption.Value;
        }

        [HttpGet("routes")]
        public async Task<IActionResult> GetRouteLogsAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize, string searchText = null)
        {
            var res = await _routeLogRepository.GetPageAsync(new DapperPager(pageIndex, pageSize));

            return new OkResponse(res);
        }

        [HttpGet("audits")]
        public async Task<IActionResult> GetAuditLogsAsync(int pageIndex = CommonVariables.pageIndex,
            int pageSize = CommonVariables.pageSize,
            string continuationToken = null,
            string searchText = null)
        {
            var res = await _auditLogRepository.GetPageAsync(new DapperPager(pageIndex, continuationToken, pageSize));

            return new OkResponse(res);
        }

        [HttpGet("exceptions")]
        public async Task<IActionResult> GetExceptionLogssAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize, string searchText = null)
        {
            var res = await _exceptionLogRepository.GetPageAsync(new DapperPager(pageIndex, pageSize));

            return new OkResponse(res);
        }

        [HttpGet("sqls")]
        public async Task<IActionResult> GetSqlLogssAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize, string searchText = null)
        {
            var res = await _sqlLogRepository.GetPageAsync(new DapperPager(pageIndex, pageSize));

            return new OkResponse(res);
        }

        [HttpGet("file/file-directories")]
        public async Task<IActionResult> GetFileLogsDirectoriesAsync()
        {
            var res = FileExtension.GetDirectories(_logOption.Provider.File.Path,true);

            return await Task.FromResult(new OkResponse(res));
        }

        [HttpGet("file/read-file")]
        public async Task<IActionResult> GetLogsFromFileAsync(string fileName
            ,int pageIndex = CommonVariables.pageIndex
            ,int pageSize = CommonVariables.pageSize)
        {
            var res = FileExtension.GetLogObjects(_logOption.Provider.File.Path,fileName).Paging(pageIndex, pageSize);

            return await Task.FromResult(new OkResponse(res));
        }

        [HttpGet("file/search-file")]
        public async Task<IActionResult> GetLogFilesBySearchKeyAsync(string searchKey)
        {
            var res = FileExtension.GetFilenames(_logOption.Provider.File.Path, searchKey);

            return await Task.FromResult(new OkResponse(res));
        }
    }
}
