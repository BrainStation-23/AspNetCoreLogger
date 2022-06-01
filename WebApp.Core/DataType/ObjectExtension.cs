using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
    }
}
