using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WebApp.Core.Middlewares
{
    /// <summary>
    /// Handling all exception message globaly with custom middleware
    /// startup.cs -> app.UseMiddleware<GlobalExceptionCustomMiddleware>();;
    /// </summary>
    public class GlobalExceptionCustomMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionCustomMiddleware> _logger;

        public GlobalExceptionCustomMiddleware(RequestDelegate next, ILogger<GlobalExceptionCustomMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                httpContext.Request.EnableBuffering();
                var bodyAsText = await new System.IO.StreamReader(httpContext.Request.Body).ReadToEndAsync();
                httpContext.Request.Body.Position = 0;

                await _next(httpContext);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(httpContext, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            await exception.ErrorAsync(context, _logger);
        }
    }
}