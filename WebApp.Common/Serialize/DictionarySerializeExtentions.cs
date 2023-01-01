using Newtonsoft.Json.Linq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WebApp.Common.DataType;

namespace WebApp.Common.Serialize
{
    public static class DictionarySerializeExtentions
    {
        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            return obj.GetType()
                .GetProperties()
                .Select(pi => new { pi.Name, Value = pi.GetValue(obj, null) })
                .Union(
                    obj.GetType()
                        .GetFields()
                        .Select(fi => new { fi.Name, Value = fi.GetValue(obj) })
                )
                .ToDictionary(ks => ks.Name, vs => vs.Value);
        }

        public static Dictionary<string, dynamic> ToDynamicDictionary(this object obj)
        {
            if (obj == null)
                return null;

            try
            {
                var a= obj.ToJson().ToModel<List<dynamic>>().ArrayToDynamicDictionary();

                return a;
            }
            catch(Exception ex) { 
            try
            {
                var dict = obj.ToJson().ToModel<Dictionary<string, dynamic>>();

                foreach (var item in dict)
                {
                    if (item.Value is not null)
                    {
                        dict[item.Key] = CheckDictionaryProperties(item.Value);
                    }
                }
                return dict;
            }
            catch (Exception exp) { return null; }
            }
        }

        public static dynamic CheckDictionaryProperties(dynamic value)
        {
            if (value == null)
                return null;
            dynamic output;
            Type type = value.GetType();
            if (type.Name.ToLower().Contains("jobject"))
            {
                object obj = value.ToObject<object>();
                output = obj.ToDynamicDictionary();
                return output;
            }
            else if (type.Name.ToLower().Contains("jarray"))
            {
                return ArrayToDynamicDictionary(value);
            }
            else
            {
                return value;
            }
        }

        public static dynamic ArrayToDynamicDictionary(this List<dynamic> values)
        {
            Dictionary<string,dynamic> Dict = new Dictionary<string, dynamic>();
            int pos = 0;

            foreach (var val in values)
            {
                Dict.Add(pos.ToString(),CheckDictionaryProperties(val));
                pos++;
            }
            return Dict;
        }
    }
}
