using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using WebApp.Core.Extensions;
using WebApp.Core.Loggers.Repositories;
using WebApp.Core.Models;

namespace WebApp.Core.Middlewares
{
    /// <summary>
    /// Handling all request
    /// startup.cs -> app.UseMiddleware<ExceptionMiddleware>();;
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IExceptionLogRepository _exceptionLogRepository;

        public ExceptionMiddleware(RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IExceptionLogRepository exceptionLogRepository)
        {
            _next = next;
            _logger = logger;
            _exceptionLogRepository = exceptionLogRepository;
        }

        public async Task InvokeAsync(HttpContext context)
        {
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
                context.Response.Body = originalBodyStream;
                var apiResponse = errorModel.ToApiResponse();
                await context.Response.WriteAsync(apiResponse);
                await _exceptionLogRepository.AddAsync(errorModel);
            }
            finally
            {
                await responseBody.DisposeAsync();
            }
        }

    }
}
