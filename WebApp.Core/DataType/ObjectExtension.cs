using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Core
{
    public static class ObjectExtension
    {
        public static object GetPropValue(this object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public static IDictionary<string, string> ToDictionary(this IFormCollection collection)
        {
            var dictionary = new Dictionary<string, string>();

            foreach (var key in collection.Keys)
            {
                dictionary.Add(key, collection[key]);
            }

            return dictionary;
        }

        public static string ReadStream(this Stream stream)
        {
            string bodyStr = string.Empty;

            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, true, 1024, true))
            {
                bodyStr = reader.ReadToEnd();
            }

            return bodyStr;
        }

        public static string ReadRequestBody(this HttpRequest request)
        {
            request.EnableBuffering();
            var bodyStr = ReadStream(request.Body);
            request.Body.Position = 0;

            return bodyStr;
        }

        //public static string ReadResponseBody(this HttpResponse response)
        //{
        //    response.
        //    var bodyStr = ReadStream(response.Body);
        //    response.Body.Position = 0;

        //    return bodyStr;
        //}

        public static async Task<string> GetBodyAsync(this HttpRequest request)
        {
            string bodyStr = string.Empty;

            request.EnableBuffering();
            using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
            {
                bodyStr = await reader.ReadToEndAsync();
            }

            request.Body.Position = 0;

            return bodyStr;


        }

        public static async Task<string> GetBody1Async(this HttpRequest request)
        {
            //if (!request.Body.CanSeek)
            //{
            //    request.EnableBuffering();
            //}

            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer.AsMemory(0, buffer.Length));
            return Encoding.UTF8.GetString(buffer);

        }

        public static async Task<string> GetReponseAsync(this HttpContext context)
        {
            string body = string.Empty;

            using (var buffer = new MemoryStream())
            {
                var stream = context.Response.Body;
                context.Response.Body = buffer;

                buffer.Seek(0, SeekOrigin.Begin);
                //var reader = new StreamReader(buffer);
                using var bufferReader = new StreamReader(buffer);
                body = await bufferReader.ReadToEndAsync();

                buffer.Seek(0, SeekOrigin.Begin);

                await buffer.CopyToAsync(stream);
                context.Response.Body = stream;

                System.Diagnostics.Debug.Print($"Response: {body}");
            }

            return body;
        }
    }
}
