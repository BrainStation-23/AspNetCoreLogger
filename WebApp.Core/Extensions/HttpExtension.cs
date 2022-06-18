using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Core.Extensions
{
    public static class HttpExtension
    {
        public static async Task<string> GetRequestBodyAsync(this HttpRequest request)
        {
            request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            await request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length));

            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Position = 0;

            return bodyAsText;
        }

        public static async Task<string> GetResponseAsync(this HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            string text = await new StreamReader(response.Body).ReadToEndAsync();

            response.Body.Seek(0, SeekOrigin.Begin);

            return text;
        }
     
        public static string ToAppStatusCode(this HttpStatusCode httpStatusCode)
        {
            int statusCode = (int)httpStatusCode;
            string appStatusCode = $"AP{statusCode + 1100}E";

            return appStatusCode;
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
    }

}
