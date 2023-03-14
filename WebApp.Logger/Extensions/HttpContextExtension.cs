using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace WebApp.Logger.Extensions
{
    public static class HttpContextExtension
    {
        private static string ToAppStatusCode(this HttpStatusCode httpStatusCode)
        {
            int statusCode = (int)httpStatusCode;
            string appStatusCode = $"AP{statusCode + 1100}E";

            return appStatusCode;
        }

        private static IDictionary<string, string> ToDictionary(this IFormCollection collection)
        {
            var dictionary = new Dictionary<string, string>();

            foreach (var key in collection.Keys)
            {
                dictionary.Add(key, collection[key]);
            }

            return dictionary;
        }

        public static long? GetUserId(this HttpContext context)
        {
            return context.User.Identity?.IsAuthenticated ?? false ? long.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier)) : null;
        }

        public static string GetIpAddress(this HttpContext context)
        {
            var ipAddress = string.Empty;

            if (context.Request.Headers is not null)
            {
                var forwardedHeader = context.Request.Headers["X-Forwarded-For"];
                if (!StringValues.IsNullOrEmpty(forwardedHeader))
                    ipAddress = forwardedHeader.FirstOrDefault();
            }

            if (string.IsNullOrEmpty(ipAddress) && context.Connection.RemoteIpAddress is not null)
                ipAddress = context.Connection.RemoteIpAddress.ToString();

            return ipAddress;
        }

        public static string GetUrl(this HttpContext context)
        {
            return context.Request.GetDisplayUrl() ?? context.Request.GetEncodedUrl();
        }

        public static string GetControllerName(this HttpContext context)
        {
            return context.Request.RouteValues["controller"]?.ToString();
        }

        public static string GetActionName(this HttpContext context)
        {
            return context.Request.RouteValues["action"]?.ToString();
        }

        public static string GetUrlReferrer(this HttpContext context)
        {
            return context.Request.Headers["Referer"].ToString();
        }

        public static string GetHttpVersion(this HttpContext context)
        {
            //return (string)context.Features.GetPropValue("HttpVersion");
            return string.Empty;
        }

        public static string GetFormValue(this HttpContext context)
        {
            return context.Request.HasFormContentType ? JsonSerializer.Serialize(context.Request.Form.ToDictionary()) : string.Empty;
        }

        public static string GetAppStatusCode(this HttpContext context)
        {
            return ((HttpStatusCode)context.Response.StatusCode).ToAppStatusCode();
        }

        public static string ToAppStatusCode(this int statusCode)
        {
            return ((HttpStatusCode)statusCode).ToAppStatusCode();
        }
    }
}
