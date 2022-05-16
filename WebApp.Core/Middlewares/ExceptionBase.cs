using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Claims;
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

            //logger.LogError(exception.Message);

            ErrorModel model = new ErrorModel();
            model.UserId = context.User.Identity?.IsAuthenticated ?? false ? long.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier)) : null;
            model.Source = exception.Source;
            model.StackTrace = exception.StackTrace;
            model.IpAddress = context.GetIpAddress();
            model.Host = context.Request.Host.ToString();
            model.Url = context.Request.GetDisplayUrl() ?? context.Request.GetEncodedUrl();
            model.Url = $"{context.Request.Method} {model.Url}";
            model.Message = exception.Message;
            model.StatusCode = (HttpStatusCode)response.StatusCode;
            model.AppStatusCode = ((HttpStatusCode)response.StatusCode).ToAppStatusCode();
            model.Application = exception.Source;
            model.Version = context.Request.Scheme;

            //var form = JsonSerializer.Serialize(context.Request.Form);
            //var body1 = JsonSerializer.Serialize(context.Request.Body);
            //var headers = JsonSerializer.Serialize(context.Request.Headers);

            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);

            return model;
        }
    }
}
