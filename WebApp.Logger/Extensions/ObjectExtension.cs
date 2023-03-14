using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebApp.Logger.Extensions
{
    public static class ObjectExtension
    {
        public static object GetPropValue(this object src, string propName)
        {
            return src.GetType().GetProperty(propName)?.GetValue(src, null);
        }

        public static bool NotContains<T>(this List<T> lists, T item)
        {
            return !lists.Contains(item);
        }

        public static List<PropertyInfo> GetProperties(this object obj)
        {
            var properties = obj.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToList();

            return properties;
        }

        public static bool IsClassObjectProperty(this PropertyInfo propertyInfo, object obj)
        {
            if (propertyInfo.PropertyType.IsClass && propertyInfo.PropertyType.Assembly.FullName == obj.GetType().Assembly.FullName)
                return true;

            return false;
        }

        public static object Iterate(this object obj, IEnumerable<Type> visited = null)
        {
            if (obj is null)
                return null;

            var properties = obj.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                var propertyValue = property.GetValue(obj, null);
                var propertyKey = property.Name;

                if (property.IsClassObjectProperty(obj))
                {
                    var isVisited = visited.Contains(property.PropertyType);

                    if (!isVisited)
                        property.Iterate(visited.Union(new Type[] { property.PropertyType }));
                }

                if (property is IEnumerable)
                {
                    IList collection = (IList)property;
                    foreach (var val in collection)
                        Iterate(val);
                }

                if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                    property.SetValue(obj, DateTime.Now, null);

                else if (property.PropertyType.IsClass && !visited.Contains(property.PropertyType))
                {
                    Iterate(property.PropertyType, visited.Union(new Type[] { property.PropertyType }));
                }
            }

            return obj;
        }

        public static object ReadNestedObject(this object obj)
        {
            if (obj == null)
                return null;

            var typeName = obj.GetType().Name.ToLower();

            //if (!HashTableSerializeExtentions.typecontainer.Contains(typeName))
            //{
            //    HashTableSerializeExtentions.typecontainer.Add(typeName);
            //}

            if (typeName.Contains("object") || typeName.Contains("dictionary"))
            {
                var hash = obj.ToJson().ToModel<Hashtable>();
                var newHash = new Hashtable();

                foreach (var key in hash.Keys)
                {
                    var newHashValue = ReadNestedObject(hash[key]);
                    newHash.Add(key, newHashValue);
                }

                return newHash;
            }
            else if (typeName.Contains("list") || typeName.Contains("array"))
            {
                var list = obj.ToJson().ToModel<List<object>>();

                if (list.Count == 0)
                    return null;

                var newlist = new List<object>();

                foreach (var val in list)
                {
                    newlist.Add(ReadNestedObject(val));
                }

                return newlist;
            }
            else
            {
                return obj;
            }
        }
    }
}
