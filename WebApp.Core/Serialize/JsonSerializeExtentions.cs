using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace WebApp.Common.Serialize
{
    public static class JsonSerializeExtentions
    {
        public static string ToJsonSerialize(this DataSet ds)
        {
            return JsonConvert.SerializeObject(ds, Formatting.None, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            //return ds.Tables[0].ToJsonSerializer();
        }

        public static string ToJsonSerialize(this DataTable dt)
        {
            return JsonConvert.SerializeObject(dt, Formatting.None, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        public static string ToJson(this object value)
        {
            if (value == null)
                return null;

            return JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                // Ignore null values while serializing
                NullValueHandling = NullValueHandling.Ignore,
                // No formatting
                Formatting = Formatting.None
            });
        }

        public static object FromJson(this string value)
        {
            return JsonConvert.DeserializeObject(value);
        }

        public static T FromJson<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,
            });
        }

        public static T ToModel<T>(this string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
        }

        public static bool IsValidJson(this string value)
        {
            try
            {
                JObject.Parse(value);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public static string JsonUnescaping(this string value)
        {
            var remover = new Dictionary<string, string> {
                { "\\", ""},
                { "\"[", "["},
                { "]\"", "]"},
                { "\\\t", "\t" },
                { "\\\n", "\n"},
                { "\\\r", "\r"},
                { "\"{", "{"},
                { "}\"", "}"},
                { "\\\"", "\""},
            };

            foreach (var remove in remover)
            {
                value = value.Replace(remove.Key, remove.Value);
            }

            value = Regex.Unescape(value);

            return value;
        }
    }
}
