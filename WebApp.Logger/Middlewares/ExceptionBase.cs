using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using WebApp.Common.DataType;
using WebApp.Common.Exceptions;
using WebApp.Common.Extensions;
using WebApp.Common.Responses;
using WebApp.Common.Serialize;
using WebApp.Logger.Extensions;
using WebApp.Logger.Models;

namespace WebApp.Logger.Middlewares
{
    public static class ExceptionBase
    {
        public static string ToApiResponse(this ErrorModel errorModel)
        {
            var apiResponse = new ApiResponse(false);
            apiResponse.StatusCode = (int)errorModel.StatusCode;
            apiResponse.AppStatusCode = errorModel.AppStatusCode;
            apiResponse.Errors = errorModel.Errors;
            apiResponse.Message = errorModel.MessageDetails ?? errorModel.Message;

            return JsonSerializer.Serialize(apiResponse);
        }

        public static string ToApiDevelopmentResponse(this ErrorModel errorModel)
        {
            var apiResponse = new
            {
                IsSuccess = false,
                StatusCode = (int)errorModel.StatusCode,
                AppStatusCode = errorModel.AppStatusCode,
                Errors = errorModel.Errors,
                Message = errorModel.Message,
                MessageDetails = errorModel.MessageDetails,
                StackTrace = errorModel.StackTrace,
                TraceId = errorModel.TraceId
            };

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
            model.Form = context.Request.HasFormContentType ? JsonSerializer.Serialize(context.Request.Form.ToDictionary()) : string.Empty;
            model.RequestHeaders = JsonSerializer.Serialize(context.Request.Headers);
            model.ResponseHeaders = JsonSerializer.Serialize(context.Request.Headers);
            model.Response = string.Empty;
            model.TraceId = context.TraceIdentifier;
            model.Scheme = context.Request.Scheme;
            model.Proctocol = context.Request.Protocol;
            model.ControllerName = context.Request.RouteValues["controller"].ToString();
            model.ApplicationName = AppDomain.CurrentDomain.FriendlyName.ToString();
            model.ActionName = context.Request.RouteValues["action"].ToString();
            model.RequestMethod = context.Request.Method;
            model.ResponseLength = context.Response.ContentLength.ToString();
            model.RequestLength = context.Request.ContentLength.ToString();
            model.IsHttps = context.Request.IsHttps;
            model.CorrelationId = context.TraceIdentifier;
            model.UrlReferrer = context.Request.Headers["Referer"].ToString();
            //model.Body = await context.Request.GetBody1Async();
            model.Version = (string)context.Features.GetPropValue("HttpVersion");
            //model.Session = context.Session;
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
            model.Scheme = context.Request.Scheme;
            model.Form = context.Request.HasFormContentType ? JsonSerializer.Serialize(context.Request.Form.ToDictionary()) : string.Empty;
            model.RequestHeaders = JsonSerializer.Serialize(context.Request.Headers);
            model.ResponseHeaders = JsonSerializer.Serialize(context.Request.Headers);
            model.Response = await context.Response.GetResponseAsync();
            model.TraceId = context.TraceIdentifier;
            model.Scheme = context.Request.Scheme;
            model.Proctocol = context.Request.Protocol;
            model.ErrorCode = exception.GetType().Name.ToShorten();
            model.ControllerName = context.Request.RouteValues["controller"].ToString();
            model.ApplicationName = AppDomain.CurrentDomain.FriendlyName.ToString();
            model.ActionName = context.Request.RouteValues["action"].ToString();
            model.RequestMethod = context.Request.Method;
            //model.Body = await context.Request.GetRequestBodyAsync();
            model.Version = (string)context.Features.GetPropValue("HttpVersion");
            //model.Url = $"{context.Request.Method} {model?.Url}";
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
            model.Scheme = context.Request.Scheme;
            model.Form = context.Request.HasFormContentType ? JsonSerializer.Serialize(context.Request.Form.ToDictionary()) : string.Empty;
            model.RequestHeaders = JsonSerializer.Serialize(context.Request.Headers);
            model.ResponseHeaders = JsonSerializer.Serialize(context.Request.Headers);
            //model.Body = await context.Request.GetBody1Async();
            model.Response = string.Empty;
            model.TraceId = context.TraceIdentifier;
            model.Version = (string)context.Features.GetPropValue("HttpVersion");
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

                case DirectoryNotFoundException:
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

                case AppException ae:
                    errorModel.StatusCode = HttpStatusCode.OK;
                    errorModel.Message = ae.Message;
                    errorModel.Errors = ae.Errors;
                    break;

                default:
                    //errorModel.StatusCode = HttpStatusCode.InternalServerError;
                    errorModel.Message = exception.Message ?? "Internal Server errors. Check Logs!";

                    break;
            }

            errorModel.AppStatusCode = ((HttpStatusCode)errorModel.StatusCode).ToAppStatusCode();

            logger.LogError(exception, exception.Message);

            return errorModel;
        }
    }
}
