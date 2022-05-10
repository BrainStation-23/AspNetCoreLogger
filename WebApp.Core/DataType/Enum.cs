using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebApp.Core.DataType
{
    public static class Enum<T> where T : struct, IConvertible
    {
        public static int Length
        {
            get
            {
                if (typeof(T).IsEnum)
                    return Enum.GetNames(typeof(T)).Length;

                throw new ArgumentException("T must be an enumerated type");
            }
        }

        public static int Count()
        {
            if (typeof(T).IsEnum)
                return Enum.GetNames(typeof(T)).Length;

            throw new ArgumentException("T must be an enumerated type");
        }

        public static IEnumerable<SelectListItem> ToSelectListItem()
        {
            var type = typeof(T);
            return Enum.GetValues(type).Cast<int>().Select(v => new SelectListItem
            {
                Text = Enum.GetName(typeof(T), v),
                Value = ((int)v).ToString()
            }).ToList();
        }

        public static SelectList ToSelectList()
        {
            Type type = typeof(T);
            if (type.IsEnum)
            {
                var values = from Enum e in Enum.GetValues(type)
                             select new { Id = (Convert.ToInt32(e).ToString()), Name = e.ToString() };
                return new SelectList(values, "Id", "Name");
            }
            return null;
        }

        public static List<T> ToList()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }
        public static List<string> ToListString()
        {
            return Enum.GetValues(typeof(T)).Cast<string>().ToList();
        }
        public static string[] ToArray()
        {
            return Enum.GetValues(typeof(T)).Cast<object>().Select(e=>e.ToString()).ToArray();
        }

        public static List<T> ToList1()
        {
            Type enumType = typeof(T);

            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");

            Array array = Enum.GetValues(enumType);

            List<T> enumValueList = new List<T>(array.Length);

            foreach (int val in array)
            {
                enumValueList.Add((T)Enum.Parse(enumType, val.ToString()));
            }

            return enumValueList;
        }

        public static IDictionary<KeyType, string> ToDictionary<KeyType>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToDictionary(e => (KeyType)(object)e, e => e.ToString());
        }

        public static IDictionary<T, string> ToDictionary()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToDictionary(e => e, e => e.ToString());
        }

        public static Dictionary<int, string> ToDictionary1()
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();

            foreach (var name in Enum.GetNames(typeof(T)))
            {
                dictionary.Add((int)Enum.Parse(typeof(T), name, true), name);
            }

            return dictionary;
        }

        public static Dictionary<int, string> ToDictionary2()
        {
            Dictionary<int, string> dictionary = new Dictionary<int, string>();

            foreach (T v in Enum.GetValues(typeof(T)))
            {
                dictionary.Add((Convert.ToInt32(v)), v.ToString());
            }

            return dictionary;
        }

        public static IDictionary<int, string> ToDictionary3()
        {
            var dictionary = new Dictionary<int, string>();

            var values = Enum.GetValues(typeof(T));

            foreach (var value in values)
            {
                int key = (int)value;

                dictionary.Add(key, value.ToString());
            }

            return dictionary;
        }

        public static string Name(int value)
        {
            return Enum.GetValues(typeof(T)).Cast<T>().FirstOrDefault(e => Convert.ToInt32(e) == value).ToString();
        }

        public static int Value(T name)
        {
            return Convert.ToInt32(name);
        }

        public static int Value(string name)
        {
            try
            {
                var value = Enum.GetValues(typeof(T)).Cast<T>().First(e => e.ToString() == name);

                return (int)(object)value;
            }
            catch (Exception)
            {
                throw new ArgumentException("invalid enum data");
            }
        }

        public static List<EnumModel> Values(Type enumType)
        {
            var model = new List<EnumModel>();
            if (enumType.IsEnum)
            {
                foreach (var e in enumType.GetEnumValues())
                {
                    var value = GetDescription(enumType, e.ToString());
                    model.Add(new EnumModel { Key = (int)e, Value = value ?? e.ToString() });
                }

                return model;
            }

            return null;
        }

        private static string GetDescription(Type enumType, string name)
        {
            var filedName = enumType.GetFields().FirstOrDefault(x => x.Name == name);
            if (filedName == null)
                return null;

            DescriptionAttribute descriptionAttribute = Attribute.GetCustomAttribute(filedName, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (descriptionAttribute == null)
                return null;

            return descriptionAttribute.Description;
        }

        /// <summary>
        /// Get enum int value by enum key
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="key">Key of the Enum</param>
        /// <returns>int value from enum</returns>
        public static int Parse(string key)
        {
            return (int)Enum.Parse(typeof(T), key, true);
        }
    }

    public class EnumModel
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
}