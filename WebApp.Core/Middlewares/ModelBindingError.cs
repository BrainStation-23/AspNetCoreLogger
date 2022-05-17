using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace WebApp.Core.Middlewares
{
    public static  class ModelBindingError
    {
        public static void ConfigureModelBindingExceptionHandling(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    ValidationProblemDetails error = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .Select(e => new ValidationProblemDetails(actionContext.ModelState)).FirstOrDefault();

                    // Here you can add logging to you log file or to your Application Insights.
                    // For example, using Serilog:
                    // Log.Error($"{{@RequestPath}} received invalid message format: {{@Exception}}", 
                    //   actionContext.HttpContext.Request.Path.Value, 
                    //   error.Errors.Values);
                    return new BadRequestObjectResult(error);
                };
            });
        }
    }
}
