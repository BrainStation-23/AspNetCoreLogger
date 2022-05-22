using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApp.Core.Extensions;
using WebApp.Core.Models;

namespace WebApp.Core.Middlewares
{
    public static class HttpExtension
    {
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

        public static async Task<string> GetRequestBodyAsync(this HttpRequest request)
        {
            var body = request.Body;

            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length));
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body = body;

            return bodyAsText;
        }

        public static async Task<string> FormatResponse(this HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            string text = await new StreamReader(response.Body).ReadToEndAsync();

            response.Body.Seek(0, SeekOrigin.Begin);

            return text;
        }

    }
}
