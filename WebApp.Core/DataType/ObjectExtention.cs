using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Core.DataType
{
    public static class ObjectExtention
    {
        public static Dictionary<string, object> ToDictionaryProperties(this object atype, BindingFlags flags)
        {
            if (atype == null) return new Dictionary<string, object>();
            var t = atype.GetType();
            var props = t.GetProperties(flags);
            var dict = new Dictionary<string, object>();
            foreach (PropertyInfo prp in props)
            {
                object value = prp.GetValue(atype, new object[] { });
                dict.Add(prp.Name, value);
            }
            return dict;
        }
        //Usage:
        //var customerValues = customer.ToDictionaryProperties(BindingFlags.Public); // Returns: a list of public properties with their values in a Dictionary<string,object>()

        public static bool IsNullOrDbNull(this object obj)
        {
            return obj == null
                   || obj.GetType() == typeof(DBNull);
        }

        public static void ThrowIfArgumentIsNull<T>(this T obj) where T : class
        {
            if (obj == null) throw new ArgumentNullException(nameof(T) + " not allowed to be null");
        }

        public static Dictionary<string, object> ToDictionary(this object o)
        {
            var dictionary = new Dictionary<string, object>();

            foreach (var propertyInfo in o.GetType().GetProperties())
            {
                if (propertyInfo.GetIndexParameters().Length == 0)
                {
                    dictionary.Add(propertyInfo.Name, propertyInfo.GetValue(o, null));
                }
            }

            return dictionary;
        }
    }
}
