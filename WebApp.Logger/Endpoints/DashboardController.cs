using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.Logger.Loggers.Providers;
using WebApp.Logger.Responses;

namespace WebApp.Logger
{
    [ApiController]
    [Route("dlw/dashboard")]
    [Produces("application/json")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetLogCountSummaryAsync()
        {
            var res = await _dashboardRepository.GetSummaryAsync();

            return new OkResponse(res);
        }

        [HttpGet("top-requests")]
        public async Task<IActionResult> GetTopRequestsAsync()
        {
            var res = await _dashboardRepository.GetTopRequestsAsync();

            return new OkResponse(res);
        }

        [HttpGet("top-exceptions")]
        public async Task<IActionResult> GetTopExceptionsAsync()
        {
            var res = await _dashboardRepository.GetTopExceptionAsync();

            return new OkResponse(res);
        }

        [HttpGet("slowest-request")]
        public async Task<IActionResult> GetSlowestRequestAsync()
        {
            var res = await _dashboardRepository.GetSlowestRequestAsync();

            return new OkResponse(res);
        }

        [HttpGet("fastest-request")]
        public async Task<IActionResult> GetFastestRequestAsync()
        {
            var res = await _dashboardRepository.GetFastestRequestAsync();

            return new OkResponse(res);
        }
    }
}
