using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApp.Core.Models;
using WebApp.Core.Responses;

namespace WebApp.Core.Middlewares
{
    /// <summary>
    /// Handling all request
    /// startup.cs -> app.UseMiddleware<HttpRequestMiddleware>();;
    /// </summary>
    public class HttpRequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpRequestMiddleware> _logger;

        public HttpRequestMiddleware(RequestDelegate next, ILogger<HttpRequestMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var errorModel = new ErrorModel();
            var requestModel = new RequestModel();

            //var originalBodyStream = context.Response.Body;
            var responseBody = new MemoryStream();

            try
            {
                requestModel = await context.ToModelAsync();
                //requestModel.Body = await context.Request.GetRequestBodyAsync();

                //context.Response.Body = responseBody;

                await _next(context);

                //requestModel.Response = await GetResponseAsync(context.Response);

                //await responseBody.CopyToAsync(originalBodyStream);
                //context.Response.Body = originalBodyStream;
            }
            catch (Exception exception)
            {
                //context.Response.Body = originalBodyStream;
                errorModel = await exception.ErrorAsync(context, _logger);
                var apiResponse = ToApiResponse(errorModel);
                await context.Response.WriteAsync(apiResponse);
            }
            finally
            {
                await responseBody.DisposeAsync();
            }
        }

        private async Task<string> GetRequestBodyAsync(HttpRequest request)
        {
            var body = request.Body;

            request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            var bodyAsText = Encoding.UTF8.GetString(buffer);

            request.Body = body;

            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {bodyAsText}";
        }

        private async Task<string> GetResponseAsync(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            string text = await new StreamReader(response.Body).ReadToEndAsync();

            response.Body.Seek(0, SeekOrigin.Begin);

            return $"{response.StatusCode}: {text}";
        }

        private string ToApiResponse(ErrorModel errorModel)
        {
            var apiResponse = new ApiResponse(false);
            apiResponse.StatusCode = (int)errorModel.StatusCode;
            apiResponse.Errors = errorModel.Errors;
            var result = JsonSerializer.Serialize(apiResponse);

            return result;
        }
    }
}
