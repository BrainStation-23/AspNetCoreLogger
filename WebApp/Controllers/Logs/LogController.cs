using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.Core;
using WebApp.Core.Loggers.Repositories;
using WebApp.Core.Responses;

namespace WebApp.Controllers.Logs
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly IRouteLogRepository _routeLogRepository;
        private readonly IExceptionLogRepository _exceptionLogRepository;

        public LogController(IRouteLogRepository routeLogRepository, IExceptionLogRepository exceptionLogRepository)
        {
            _routeLogRepository = routeLogRepository;
            _exceptionLogRepository = exceptionLogRepository;
        }

        [HttpGet("routes")]
        public async Task<IActionResult> GetRoutesAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize, string searchText = null)
        {
            var res = await _routeLogRepository.GetPageAsync(new DapperPager(pageIndex, pageSize));

            return new OkResponse(res);
        }
    }
}
