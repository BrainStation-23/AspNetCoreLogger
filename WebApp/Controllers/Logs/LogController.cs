using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Common.Responses;
using WebApp.Core;
using WebApp.Logger.Extensions;
using WebApp.Logger.Loggers;
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
        private readonly IServiceProvider _serviceProvider;
        private readonly LogOption _logOption;

        public LogController(IRouteLogRepository routeLogRepository,
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

            var factory = new ProviderFactory(_serviceProvider);

            var providerType = _logOption.ProviderType.ToProviderTypeEnums().FirstOrDefault();
            ILog loggerWrapper = factory.Build(providerType);

            var res = await loggerWrapper.Audit.GetPageAsync(new DapperPager(pageIndex, continuationToken, pageSize));

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
