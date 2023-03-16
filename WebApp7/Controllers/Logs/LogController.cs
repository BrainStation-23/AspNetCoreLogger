//using Microsoft.AspNetCore.Mvc;
//using WebApp.Common.Responses;
//using WebApp.Core;
//using WebApp.Logger.Contracts;
//using WebApp.Logger.Providers.Sqls;

//namespace WebApp7.Controllers.Logs
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class LogController : ControllerBase
//    {
//        private readonly ISqlLogRepository _sqlLogRepository;
//        private readonly IRequestLogRepository _routeLogRepository;
//        private readonly IErrorLogRepository _exceptionLogRepository;
//        private readonly IAuditLogRepository _auditLogRepository;
//        private readonly IServiceProvider _serviceProvider;

//        public LogController(IRequestLogRepository routeLogRepository,
//            IErrorLogRepository exceptionLogRepository,
//            IAuditLogRepository auditLogRepository,
//            ISqlLogRepository sqlLogRepository,
//            IServiceProvider serviceProvider)
//        {
//            _routeLogRepository = routeLogRepository;
//            _exceptionLogRepository = exceptionLogRepository;
//            _auditLogRepository = auditLogRepository;
//            _sqlLogRepository = sqlLogRepository;
//            _serviceProvider = serviceProvider;
//        }

//        [HttpGet("routes")]
//        public async Task<IActionResult> GetRouteLogsAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize, string? searchText = null)
//        {
//            var res = await _routeLogRepository.GetPageAsync(new DapperPager(pageIndex, pageSize));

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
//            var res = await _exceptionLogRepository.GetPageAsync(new DapperPager(pageIndex, pageSize));

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
