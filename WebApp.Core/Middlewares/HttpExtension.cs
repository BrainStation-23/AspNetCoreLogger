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
        

        //public static async Task<string> GetRequestBodyAsync(this HttpRequest request)
        //{
        //    //var body = request.Body;

        //    request.EnableBuffering();
        //    var buffer = new byte[Convert.ToInt32(request.ContentLength)];
        //    await request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length));
        //    var bodyAsText = Encoding.UTF8.GetString(buffer);
        //    request.Body.Position = 0;
        //    //request.Body = body;

        //    return bodyAsText;
        //}

        public static async Task<string> FormatResponse(this HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);

            string text = await new StreamReader(response.Body).ReadToEndAsync();

            response.Body.Seek(0, SeekOrigin.Begin);

            return text;
        }
    }
}
