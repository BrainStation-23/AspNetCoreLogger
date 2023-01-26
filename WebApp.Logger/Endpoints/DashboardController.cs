using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using WebApp.Common.Responses;
using WebApp.Core;
using WebApp.Logger.Extensions;
using WebApp.Logger.Loggers;
using WebApp.Logger.Loggers.Providers;
using WebApp.Logger.Loggers.Repositories;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger
{
    [ApiController]
    [Route("dlw/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        [HttpGet("audit/{startDateTime}/{endDateTime}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAuditLogByDateAsync(DateTime startDateTime,DateTime endDateTime,int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize)
        {
            var res =await _dashboardRepository.GetAuditPageByDateAsync(startDateTime, endDateTime, new DapperPager(pageIndex, pageSize));

            return new OkResponse(res);
        }

        [HttpGet("audit/{TraceId}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAuditLogByDateAsync(string TraceId)
        {
            var res = await _dashboardRepository.GetAuditPageByDateAsync(TraceId);

            return new OkResponse(res);
        }

        [HttpGet("sql/{startDateTime}/{endDateTime}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetSqlLogByDateAsync(DateTime startDateTime, DateTime endDateTime, int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize)
        {
            var res = await _dashboardRepository.GetSqlPageByDateAsync(startDateTime, endDateTime, new DapperPager(pageIndex, pageSize));

            return new OkResponse(res);
        }

        [HttpGet("sql/{TraceId}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetSqlLogByDateAsync(string TraceId)
        {
            var res = await _dashboardRepository.GetSqlPageByDateAsync(TraceId);

            return new OkResponse(res);
        }

        [HttpGet("error/{startDateTime}/{endDateTime}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetErrorLogByDateAsync(DateTime startDateTime, DateTime endDateTime, int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize)
        {
            var res = await _dashboardRepository.GetErrorPageByDateAsync(startDateTime, endDateTime, new DapperPager(pageIndex, pageSize));

            return new OkResponse(res);
        }

        [HttpGet("error/{TraceId}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetErrorLogByDateAsync(string TraceId)
        {
            var res = await _dashboardRepository.GetErrorPageByDateAsync(TraceId);

            return new OkResponse(res);
        }

        [HttpGet("request/{startDateTime}/{endDateTime}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetRequestLogByDateAsync(DateTime startDateTime, DateTime endDateTime, int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize)
        {
            var res = await _dashboardRepository.GetRequestPageByDateAsync(startDateTime, endDateTime, new DapperPager(pageIndex, pageSize));

            return new OkResponse(res);
        }

        [HttpGet("request/{TraceId}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetRequestLogByDateAsync(string TraceId)
        {
            var res = await _dashboardRepository.GetRequestPageByDateAsync(TraceId);

            return new OkResponse(res);
        }

        [HttpGet("log-count-summary")]
        [Produces("application/json")]
        public async Task<IActionResult> GetLogCountSummaryAsync()
        {
            var res = await _dashboardRepository.GetLogCountSummaryAsync();

            return new OkResponse(res);
        }

        [HttpGet("top-requests")]
        [Produces("application/json")]
        public async Task<IActionResult> GetTopRequestsAsync()
        {
            var res = await _dashboardRepository.GetTopRequestsAsync();

            return new OkResponse(res);
        }

        [HttpGet("top-exceptions")]
        [Produces("application/json")]
        public async Task<IActionResult> GetTopExceptionsAsync()
        {
            var res = await _dashboardRepository.GetTopExceptionAsync();

            return new OkResponse(res);
        }

        [HttpGet("slowest-request")]
        [Produces("application/json")]
        public async Task<IActionResult> GetSlowestRequestAsync()
        {
            var res = await _dashboardRepository.GetSlowestRequestAsync();

            return new OkResponse(res);
        }

        [HttpGet("fastest-request")]
        [Produces("application/json")]
        public async Task<IActionResult> GetFastestRequestAsync()
        {
            var res = await _dashboardRepository.GetFastestRequestAsync();

            return new OkResponse(res);
        }
    }
}


        
