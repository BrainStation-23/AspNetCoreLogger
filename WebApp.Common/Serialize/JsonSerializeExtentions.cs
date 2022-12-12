using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Data;
using System.Text;
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
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
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
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var remover = new Dictionary<string, string> {
                { "\"{", "{"},          //  "{      -   {
                { "}\"", "}"},           //  }"      -   }
                { "\"[", "["},          //  "[      -   [
                { "]\"", "]"},         //  ]"      -   ]
                { "\\\"\\\"", "\"\""},  //  \"\"     -   "" 
                { "\"\\\"", "\""},      //  "\"     -   " 
                { "\\\"\"", "\""},      //  \""     -   "
                { "\\\"", "\""},        //  \"      -   "
               
                //{ "\\\t", "\t" },       //  \t      -   t
                //{ "\\\n", "\n"},        //  \n      -   n
                //{ "\\\r", "\r"},        //  \r      -   r
                
            };

            foreach (var remove in remover)
            {
                value = value.Replace(remove.Key, remove.Value);
            }
           
            value = Regex.Unescape(value);

            return value;
        }

        public static string ToEncodeBase64(this string value)
        {
            if (string.IsNullOrEmpty(value)) return null;

            return WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(value));
        }

        public static string ToDecodeBase64(this string base64Value)
        {
            if (string.IsNullOrEmpty(base64Value)) return null;

            return Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(base64Value));
        }
    }
}
