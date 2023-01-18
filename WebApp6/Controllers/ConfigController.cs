using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using WebApp.Common.Contexts;
using WebApp.Common.Responses;
using WebApp.Logger.Extensions;
using WebApp.Logger.Loggers;

namespace WebApp6.Controllers
{
    //[ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [Route("c/[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly IWebHostEnvironment WebHostEnvironment;
        private readonly LogOption _logOption;

        public ConfigController(IConfiguration _configuration,
            IWebHostEnvironment webHostEnvironment,
            IOptions<LogOption> logOption)
        {
            Configuration = _configuration;
            WebHostEnvironment = webHostEnvironment;
            _logOption = logOption.Value;
        }

        [HttpGet]
        [Route("log")]
        public IActionResult GetLogOption()
        {
            var logOption = _logOption.ToJson().ToModel<LogOption>();
            logOption.Provider.CosmosDb.Key = logOption.Provider.CosmosDb.Key.MaskMe();

            return new OkResponse(logOption);
        }

        [HttpGet]
        [Route("log/valid")]
        public IActionResult Validate()
        {
            (bool valid, string errors) = LogOptionExtension.Valid(Configuration);

            return new OkResponse(new { valid, errors });
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = Configuration.GetDatabaseInfo();

            return new OkResponse(model);
        }

        [HttpGet]
        [Route("environment")]
        public IActionResult GetEnvironment(string environmentName)
        {
            var model = new
            {
                Process = Environment.GetEnvironmentVariable(environmentName, EnvironmentVariableTarget.Process),
                Machine = Environment.GetEnvironmentVariable(environmentName, EnvironmentVariableTarget.Machine),
                User = Environment.GetEnvironmentVariable(environmentName, EnvironmentVariableTarget.User)
            };

            return new OkResponse(model);
        }

        [HttpGet]
        [Route("environment/set")]
        public IActionResult SetEnvironment(string environmentName, string value)
        {
            //Environment.SetEnvironmentVariable(environmentName, value, EnvironmentVariableTarget.Machine);
            Environment.SetEnvironmentVariable(environmentName, value, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable(environmentName, value, EnvironmentVariableTarget.User);

            var model = new
            {
                Process = Environment.GetEnvironmentVariable(environmentName, EnvironmentVariableTarget.Process),
                Machine = Environment.GetEnvironmentVariable(environmentName, EnvironmentVariableTarget.Machine),
                User = Environment.GetEnvironmentVariable(environmentName, EnvironmentVariableTarget.User)
            };

            return new OkResponse(model);
        }
    }
}
// ConnectionStrings__WebAppConnection
// https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-environment-variables
