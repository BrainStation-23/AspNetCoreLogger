using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebApp.Logger.Extensions
{
    public static class JsonExtension
    {
        public static string ReadJson(string path)
        {
            string json = string.Empty;

            using (StreamReader r = new StreamReader(path))
            {
                json = r.ReadToEnd();
            }

            return json;
        }

        public static List<JProperty> SelectJsonProperties(JObject json, string[] selectProperties)
        {
            var select = json.Descendants()
                .OfType<JProperty>()
                .Where(prop => selectProperties.Select(e => e.ToLower()).Contains(prop.Name.ToLower()))
                .ToList();

            return select;
        }

        public static string ReadJsonProperties(string jsonText, string[] ignoreProperties)
        {
            JObject jObject = JObject.Parse(jsonText);
            var toRemove = SelectJsonProperties(jObject, ignoreProperties);

            foreach (var prop in toRemove)
                prop.Remove();

            var json = jObject.ToString();

            return json;
        }

        public static string MaskJsonProperties(string jsonText, string[] mastProperties)
        {
            JObject jObject = JObject.Parse(jsonText);
            var toMask = SelectJsonProperties(jObject, mastProperties);

            foreach (var prop in toMask)
                prop.Value = "*****";

            var json = jObject.ToString();

            return json;
        }
    }
}
