using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using WebApp.Logger.Extensions;
using WebApp.Logger.Loggers;
using WebApp.Logger.Models;

namespace WebApp.Logger.Middlewares
{
    /// <summary>
    /// Handling all request
    /// startup.cs -> app.UseMiddleware<ExceptionMiddleware>();;
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _webHostEnvironment;

        public ExceptionMiddleware(RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment webHostEnvironment)
        {
            _next = next;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Items["request-begin-time"] = DateTime.Now;

            var errorModel = new ErrorModel();
            var requestModel = new RequestModel();

            var originalBodyStream = context.Response.Body;
            var responseBody = new MemoryStream();

            try
            {
                requestModel = await context.ToModelAsync();
                requestModel.Body = await context.Request.GetRequestBodyAsync();

                context.Response.Body = responseBody;

                await _next(context);

                requestModel.Response = await context.Response.GetResponseAsync();

                await responseBody.CopyToAsync(originalBodyStream);
                context.Response.Body = originalBodyStream;
            }
            catch (Exception exception)
            {
                errorModel = await exception.ErrorAsync(context, _logger);
                errorModel.Body = requestModel.Body;
                context.Response.Body = originalBodyStream;

                var apiResponse = _webHostEnvironment.IsDevelopment() ? errorModel.ToApiDevelopmentResponse() : errorModel.ToApiResponse();
                await context.Response.WriteAsync(apiResponse);

                var requestbegin = (DateTime)context.Items["request-begin-time"];
                var duration = DateTime.Now - requestbegin;
                errorModel.Duration = duration.TotalMilliseconds;

                await BatchLoggingContext.PublishAsync(errorModel,LogType.Error.ToString());
            }
            finally
            {
                await responseBody.DisposeAsync();
            }
        }
    }
}
