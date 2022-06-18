using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using WebApp.Core.DataType;
using WebApp.Core.Extensions;
using WebApp.Core.Models;
using WebApp.Core.Responses;
using WebApp.Core.Exceptions;

namespace WebApp.Core.Middlewares
{
    public static class ExceptionBase
    {
        public static string ToApiResponse(this ErrorModel errorModel)
        {
            var apiResponse = new ApiResponse(false);
            apiResponse.StatusCode = (int)errorModel.StatusCode;
            apiResponse.AppStatusCode = errorModel.AppStatusCode;
            apiResponse.Errors = errorModel.Errors;
            apiResponse.Message = errorModel.Message;
            apiResponse.MessageDetails = errorModel.MessageDetails;

            return JsonSerializer.Serialize(apiResponse);
        }

        private static IEnumerable<string> ToList(this SqlErrorCollection sqlErrorCollection)
        {
            foreach (SqlError error in sqlErrorCollection)
            {
                yield return error.ToString();
            }
        }

        public static async Task<RequestModel> ToModelAsync(this HttpContext context)
        {
            var model = new RequestModel();
            model.UserId = context.User.Identity?.IsAuthenticated ?? false ? long.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier)) : null;
            model.IpAddress = context.GetIpAddress();
            model.Host = context.Request.Host.ToString();
            model.Url = context.Request.GetDisplayUrl() ?? context.Request.GetEncodedUrl();
            model.StatusCode = (HttpStatusCode)context.Response.StatusCode;
            model.AppStatusCode = ((HttpStatusCode)context.Response.StatusCode).ToAppStatusCode();
            model.Version = context.Request.Scheme;
            model.Form = context.Request.HasFormContentType ? JsonSerializer.Serialize(context.Request.Form.ToDictionary()) : string.Empty;
            model.RequestHeaders = JsonSerializer.Serialize(context.Request.Headers);
            model.ResponseHeaders = JsonSerializer.Serialize(context.Request.Headers);
            //model.Body = await context.Request.GetBody1Async();
            model.Response = string.Empty;
            model.TraceId = context.TraceIdentifier;
            //model.Version = context.Features.HttpVersion;
            model.Scheme = context.Request.Scheme;
            model.Proctocol = context.Request.Protocol;
            model.Url = $"{context.Request.Method} {model?.Url}";

            return await Task.FromResult(model);
        }

        private static async Task<ErrorModel> ToErrorModelAsync(this HttpContext context, Exception exception)
        {
            var model = new ErrorModel();
            model.UserId = context.User.Identity?.IsAuthenticated ?? false ? long.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier)) : null;
            model.Source = exception.Source;
            model.StackTrace = exception.StackTrace;
            model.IpAddress = context.GetIpAddress();
            model.Host = context.Request.Host.ToString();
            model.Url = context.Request.GetDisplayUrl() ?? context.Request.GetEncodedUrl();
            model.Message = exception.Message;
            model.StatusCode = (HttpStatusCode)context.Response.StatusCode;
            model.AppStatusCode = ((HttpStatusCode)context.Response.StatusCode).ToAppStatusCode();
            model.Application = exception.Source;
            model.Version = context.Request.Scheme;
            model.Form = context.Request.HasFormContentType ? JsonSerializer.Serialize(context.Request.Form.ToDictionary()) : string.Empty;
            model.RequestHeaders = JsonSerializer.Serialize(context.Request.Headers);
            model.ResponseHeaders = JsonSerializer.Serialize(context.Request.Headers);
            model.Body = await context.Request.GetRequestBodyAsync();
            //model.Response = await context.Response.GetResponseAsync();
            model.TraceId = context.TraceIdentifier;
            //model.Version = context.Features.HttpVersion;
            model.Scheme = context.Request.Scheme;
            model.Proctocol = context.Request.Protocol;
            model.Url = $"{context.Request.Method} {model?.Url}";
            model.ErrorCode = exception.GetType().Name.ToShorten();

            return await Task.FromResult(model);
        }

        public static async Task<ErrorModel> ToErrorModelsAsync(this HttpContext context)
        {
            var model = new ErrorModel();
            model.UserId = context.User.Identity?.IsAuthenticated ?? false ? long.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier)) : null;
            model.IpAddress = context.GetIpAddress();
            model.Host = context.Request.Host.ToString();
            model.Url = context.Request.GetDisplayUrl() ?? context.Request.GetEncodedUrl();
            model.StatusCode = (HttpStatusCode)context.Response.StatusCode;
            model.AppStatusCode = ((HttpStatusCode)context.Response.StatusCode).ToAppStatusCode();
            model.Version = context.Request.Scheme;
            model.Form = context.Request.HasFormContentType ? JsonSerializer.Serialize(context.Request.Form.ToDictionary()) : string.Empty;
            model.RequestHeaders = JsonSerializer.Serialize(context.Request.Headers);
            model.ResponseHeaders = JsonSerializer.Serialize(context.Request.Headers);
            //model.Body = await context.Request.GetBody1Async();
            model.Response = string.Empty;
            model.TraceId = context.TraceIdentifier;
            //model.Version = context.Features.HttpVersion;
            model.Scheme = context.Request.Scheme;
            model.Proctocol = context.Request.Protocol;
            model.Url = $"{context.Request.Method} {model?.Url}";

            return await Task.FromResult(model);
        }

        public static async Task<ErrorModel> ErrorAsync(this Exception exception, HttpContext context, ILogger logger)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var errorModel = await context.ToErrorModelAsync(exception);
            errorModel.MessageDetails = exception.ToInnerExceptionMessage();

            switch (exception)
            {
                case ApplicationException e:
                    if (e.Message.Contains("Invalid token"))
                    {
                        errorModel.StatusCode = HttpStatusCode.Forbidden;
                        errorModel.Message = e.Message;

                        break;
                    }

                    errorModel.StatusCode = HttpStatusCode.BadRequest;
                    errorModel.Message = e.Message;

                    break;

                case KeyNotFoundException e:
                    errorModel.StatusCode = HttpStatusCode.NotFound;
                    errorModel.Message = e.Message;

                    break;

                case SqlException e:
                    errorModel.StatusCode = HttpStatusCode.NotFound;
                    errorModel.Message = e.Message;
                    errorModel.Errors = e.Errors.ToList();

                    break;

                case DirectoryNotFoundException e:
                case DivideByZeroException:
                case DriveNotFoundException:
                case FileNotFoundException:
                case UriFormatException:
                case FormatException:
                case IndexOutOfRangeException:
                case ObjectDisposedException:
                case InvalidOperationException:
                case PlatformNotSupportedException:
                case NotImplementedException:
                case NotSupportedException:
                case OverflowException:
                case PathTooLongException:
                case RankException:
                case TimeoutException:
                case ArgumentOutOfRangeException:
                case ArgumentNullException:
                case ArgumentException:
                    errorModel.StatusCode = HttpStatusCode.NotFound;
                    errorModel.Message = exception.Message;
                    break;

                default:
                    //errorModel.StatusCode = HttpStatusCode.InternalServerError;
                    errorModel.Message = exception.Message ?? "Internal Server errors. Check Logs!";

                    break;
            }

            errorModel.AppStatusCode = ((HttpStatusCode)errorModel.StatusCode).ToAppStatusCode();

            logger.LogError(exception.Message);

            return errorModel;
        }
    }
}
