using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
using WebApp.Logger.Extensions;
using WebApp.Logger.Loggers.Repositories;
using WebApp.Logger.Models;

namespace WebApp.Logger.Middlewares
{
    /// <summary>
    /// Handling all request
    /// startup.cs -> app.UseMiddleware<HttpRequestMiddleware>();;
    /// </summary>
    public class HttpRequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpRequestMiddleware> _logger;

        public HttpRequestMiddleware(RequestDelegate next,
            ILogger<HttpRequestMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IRouteLogRepository routeLogRepository)
        {
            var requestModel = new RequestModel();

            var originalBodyStream = context.Response.Body;
            var responseBody = new MemoryStream();

            requestModel = await context.ToModelAsync();
            requestModel.Body = await context.Request.GetRequestBodyAsync();

            context.Response.Body = responseBody;

            await _next(context);

            requestModel.Response = await context.Response.GetResponseAsync();

            await responseBody.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;

            await responseBody.DisposeAsync();

            await routeLogRepository.AddAsync(requestModel);
        }
    }
}
