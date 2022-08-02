using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.Common.Responses;
using WebApp.Core;
using WebApp.Logger.Loggers.Repositories;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Controllers.Logs
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly IRouteLogRepository _routeLogRepository;
        private readonly IExceptionLogRepository _exceptionLogRepository;
        private readonly IAuditLogRepository _auditLogRepository;

        public LogController(IRouteLogRepository routeLogRepository,
            IExceptionLogRepository exceptionLogRepository,
            IAuditLogRepository auditLogRepository)
        {
            _routeLogRepository = routeLogRepository;
            _exceptionLogRepository = exceptionLogRepository;
            _auditLogRepository = auditLogRepository;
        }

        [HttpGet("routes")]
        public async Task<IActionResult> GetRouteLogsAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize, string searchText = null)
        {
            var res = await _routeLogRepository.GetPageAsync(new DapperPager(pageIndex, pageSize));

            return new OkResponse(res);
        }

        [HttpGet("audits")]
        public async Task<IActionResult> GetAuditLogsAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize, string searchText = null)
        {
            var res = await _auditLogRepository.GetPageAsync(new DapperPager(pageIndex, pageSize));

            return new OkResponse(res);
        }

        [HttpGet("exceptions")]
        public async Task<IActionResult> GetExceptionLogssAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize, string searchText = null)
        {
            var res = await _exceptionLogRepository.GetPageAsync(new DapperPager(pageIndex, pageSize));

            return new OkResponse(res);
        }
    }
}
