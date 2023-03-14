using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;
using WebApp.Logger.Extensions;
using WebApp.Logger.Loggers;

namespace WebApp.Logger.Middlewares
{
    public class HttpRequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpRequestMiddleware> _logger;
        private readonly LogOption _logOptions;

        public HttpRequestMiddleware(RequestDelegate next,
            ILogger<HttpRequestMiddleware> logger,
            IOptions<LogOption> logOptions)
        {
            _next = next;
            _logger = logger;
            _logOptions = logOptions.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var isSkipable = LogOptionExtension.SkipRequest(context, _logOptions);
            if (isSkipable)
            {
                await _next(context);
                return;
            }


            var originalBodyStream = context.Response.Body;
            var responseBody = new MemoryStream();

            var requestModel = await context.ToModelAsync();
            requestModel.Body = await context.Request.GetRequestBodyAsync();

            context.Response.Body = responseBody;

            await _next(context);

            requestModel.Response = await context.Response.GetResponseAsync();

            await responseBody.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;

            await responseBody.DisposeAsync();

            //var factory = new ProviderFactory(_serviceProvider);
            ////var providerType = _logOptions.ProviderType.ToProviderTypeEnums().FirstOrDefault();
            //ILog loggerWrapper = factory.Build(_logOptions.ProviderType);
            var requestbegin = (DateTime)context.Items["request-begin-time"];
            var duration = DateTime.Now - requestbegin;
            requestModel.Duration = duration.TotalMilliseconds;

            var request = requestModel.PrepareRequestModel(_logOptions);
            //await loggerWrapper.Request.AddAsync(request);
            //await RouteLogRepository.AddAsync(request);

            await BatchLoggingContext.PublishAsync(request,LogType.Request.ToString());

            //await routeLogRepository.AddAsync(requestModel);
        }
    }
}
