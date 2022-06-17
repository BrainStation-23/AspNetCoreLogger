using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using System;
using System.Security.AccessControl;
using WebApp.Core.Contexts;
using WebApp.Core.Responses;

namespace WebApp.Controllers
{
    //[ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [Route("c/[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly IWebHostEnvironment WebHostEnvironment;

        public ConfigController(IConfiguration _configuration,
            IWebHostEnvironment webHostEnvironment)
        {
            Configuration = _configuration;
            WebHostEnvironment = webHostEnvironment;
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
