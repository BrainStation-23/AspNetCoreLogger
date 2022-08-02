using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WebApp.Logger.Middlewares
{
    public class HttpRequestHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpRequestHeaderMiddleware> _logger;

        public HttpRequestHeaderMiddleware(RequestDelegate next,
            ILogger<HttpRequestHeaderMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {

            context.Response.GetTypedHeaders().CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
            {
                Public = true,
                MaxAge = TimeSpan.FromSeconds(10)
            };
            context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] = new string[] { "Accept-Encoding" };
            context.Response.Headers["Application"] = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            context.Response.Headers["Version"] = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            await _next(context);
        }
    }
}
