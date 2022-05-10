using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace WebApp.Common.Serialize
{
    public static class JsonSerializeExtentions
    {
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
    }
}
