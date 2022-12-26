using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
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

        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        public static string ToPrettyJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        public static T ToModel<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str, new JsonSerializerSettings
            {
                DateParseHandling = DateParseHandling.None
            });
        }

        public static List<JProperty> SelectJsonProperties(JObject json, string[] selectProperties)
        {
            var select = json.Descendants()
                .OfType<JProperty>()
                .Where(prop => selectProperties.Select(e => e.ToLower()).Contains(prop.Name.ToLower()))
                .ToList();

            return select;
        }

        public static string SkipIt(this string jsonText, string[] ignoreProperties)
        {
            JObject jObject = JObject.Parse(jsonText);
            var toRemove = SelectJsonProperties(jObject, ignoreProperties);

            foreach (var prop in toRemove)
                prop.Remove();

            var json = jObject.ToString();

            return json;
        }

        public static string MaskIt(this string jsonText, string[] maskProperties)
        {
            JObject jObject = JObject.Parse(jsonText);
            var toMask = SelectJsonProperties(jObject, maskProperties);

            foreach (var prop in toMask)
            {
                if(prop.Value.Type == JTokenType.String)
                {
                    prop.Value = "*****";
                }
            }

            var json = jObject.ToString();

            return json;
        }

        public static T ToFilter<T>(this object obj, string[] skipColumns, string[] maskColumns)
        {
            if (obj == null)
                return default(T);

            return obj.ToJson()
                .SkipIt(skipColumns)
                .MaskIt(maskColumns)
                .ToModel<T>();
        }

        public static object ToFilter(this object obj, string[] skipColumns, string[] maskColumns)
        {
            if (obj == null)
                return null;

            var data = obj.ToJson()
                .SkipIt(skipColumns)
                .MaskIt(maskColumns);

            return JsonConvert.DeserializeObject(data, new JsonSerializerSettings
            {
                DateParseHandling = DateParseHandling.None
            });
        }

    }
}
