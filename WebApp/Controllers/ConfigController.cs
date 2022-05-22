using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebApp.Core.Contexts;
using WebApp.Core.Responses;

namespace WebApp.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [Route("c/[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration Configuration;

        public ConfigController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        public IActionResult Index()
        {
            var model = Configuration.GetDatabaseInfo();

            return new OkResponse(model);
        }
    }
}
