//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Options;
//using WebApp.Common.Responses;
//using WebApp.Core;
//using WebApp.Logger.Contracts;
//using WebApp.Logger.Loggers;
//using WebApp.Logger.Providers.Sqls;

//namespace WebApp6.Controllers.Logs
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class LogController : ControllerBase
//    {
//        private readonly IRequestLogRepository _requestLogRepository;
//        private readonly IErrorLogRepository _errorLogRepository;
//        private readonly IAuditLogRepository _auditLogRepository;
//        private readonly ISqlLogRepository _sqlLogRepository;
//        private readonly IServiceProvider _serviceProvider;
//        private readonly LogOption _logOption;

//        public LogController(IRequestLogRepository requestLogRepository,
//            IErrorLogRepository errorLogRepository,
//            IAuditLogRepository auditLogRepository,
//            IServiceProvider serviceProvider,
//            IOptions<LogOption> options,
//            ISqlLogRepository sqlLogRepository)
//        {
//            _requestLogRepository = requestLogRepository;
//            _errorLogRepository = errorLogRepository;
//            _auditLogRepository = auditLogRepository;
//            _serviceProvider = serviceProvider;
//            _logOption = options.Value;
//            _sqlLogRepository = sqlLogRepository;
//        }

//        [HttpGet("routes")]
//        public async Task<IActionResult> GetRouteLogsAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize, string? searchText = null)
//        {
//            var res = await _requestLogRepository.GetPageAsync(new DapperPager(pageIndex, pageSize));

//            return new OkResponse(res);
//        }

//        [HttpGet("audits")]
//        public async Task<IActionResult> GetAuditLogsAsync(int pageIndex = CommonVariables.pageIndex,
//            int pageSize = CommonVariables.pageSize,
//            string? continuationToken = null,
//            string? searchText = null)
//        {
//            var res = await _auditLogRepository.GetPageAsync(new DapperPager(pageIndex, continuationToken, pageSize));

//            return new OkResponse(res);
//        }

//        [HttpGet("exceptions")]
//        public async Task<IActionResult> GetExceptionLogssAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize, string? searchText = null)
//        {
//            var res = await _errorLogRepository.GetPageAsync(new DapperPager(pageIndex, pageSize));

//            return new OkResponse(res);
//        }

//        [HttpGet("sqls")]
//        public async Task<IActionResult> GetSqlLogssAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize, string? searchText = null)
//        {
//            var res = await _sqlLogRepository.GetPageAsync(new DapperPager(pageIndex, pageSize));

//            return new OkResponse(res);
//        }
//    }
//}
