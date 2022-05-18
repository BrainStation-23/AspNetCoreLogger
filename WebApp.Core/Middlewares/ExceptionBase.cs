using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApp.Core.Extensions;
using WebApp.Core.Models;
using WebApp.Core.Responses;

namespace WebApp.Core.Middlewares
{
    public static class ExceptionBase
    {
        public static async Task<ErrorModel> ErrorAsync(this Exception exception, HttpContext context, ILogger logger)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var errorResponse = new ApiResponse(false);

            switch (exception)
            {
                case ApplicationException e:
                    if (e.Message.Contains("Invalid token"))
                    {
                        response.StatusCode = (int)HttpStatusCode.Forbidden;
                        errorResponse.Message = e.Message;
                        break;
                    }
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = e.Message;

                    break;
                case KeyNotFoundException e:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.Message = e.Message;

                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = "Internal Server errors. Check Logs!";

                    break;
            }

            logger.LogError(exception.Message);

            ErrorModel model = new ErrorModel
            {
                UserId = context.User.Identity?.IsAuthenticated ?? false ? long.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier)) : null,
                Source = exception.Source,
                StackTrace = exception.StackTrace,
                IpAddress = context.GetIpAddress(),
                Host = context.Request.Host.ToString(),
                Url = context.Request.GetDisplayUrl() ?? context.Request.GetEncodedUrl(),
                Message = exception.Message,
                StatusCode = (HttpStatusCode)response.StatusCode,
                AppStatusCode = ((HttpStatusCode)response.StatusCode).ToAppStatusCode(),
                Application = exception.Source,
                Version = context.Request.Scheme,
                Form = JsonSerializer.Serialize(context.Request.Form),
                Headers = JsonSerializer.Serialize(context.Request.Headers)
            };
            model.Url = $"{context.Request.Method} {model?.Url}";


            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);

            return model;
        }
    }
}
