using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using WebApp.Logger.Extensions;
using WebApp.Logger.Loggers.Repositories;
using WebApp.Logger.Models;
using Microsoft.Extensions.Hosting;
using WebApp.Logger.Loggers;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

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
            IHostEnvironment webHostEnvironment
            //,IExceptionLogRepository exceptionLogRepository
            )
        {
            _next = next;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider _serviceProvider)
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
                errorModel.Body = requestModel.Body;
                context.Response.Body = originalBodyStream;

                var apiResponse = _webHostEnvironment.IsDevelopment() ? errorModel.ToApiDevelopmentResponse(): errorModel.ToApiResponse();
                await context.Response.WriteAsync(apiResponse);

                //var factory = new ProviderFactory(_serviceProvider);
                //var providerType = _logOptions.ProviderType;
                //ILog loggerWrapper = factory.Build(providerType);

                //await loggerWrapper.Error.AddAsync(errorModel);

                var _exceptionLogRepository = _serviceProvider.GetService<IExceptionLogRepository>();
               
                await _exceptionLogRepository.AddAsync(errorModel);
            }
            finally
            {
                await responseBody.DisposeAsync();
            }
        }

    }
}
