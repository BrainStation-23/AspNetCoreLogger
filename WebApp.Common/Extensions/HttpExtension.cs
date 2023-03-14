using System.Net;

namespace WebApp.Common.Extensions
{
    public static class HttpExtension
    {
        public static string ToAppStatusCode(this HttpStatusCode httpStatusCode)
        {
            int statusCode = (int)httpStatusCode;
            string appStatusCode = $"AP{statusCode + 1100}E";

            return appStatusCode;
        }

        //public static string GetIpAddress(this HttpContext context)
        //{
        //    var ipAddress = string.Empty;

        //    if (context.Request.Headers is not null)
        //    {
        //        var forwardedHeader = context.Request.Headers["X-Forwarded-For"];
        //        if (!StringValues.IsNullOrEmpty(forwardedHeader))
        //            ipAddress = forwardedHeader.FirstOrDefault();
        //    }

        //    if (string.IsNullOrEmpty(ipAddress) && context.Connection.RemoteIpAddress is not null)
        //        ipAddress = context.Connection.RemoteIpAddress.ToString();

        //    return ipAddress;
        //}
    }
}
