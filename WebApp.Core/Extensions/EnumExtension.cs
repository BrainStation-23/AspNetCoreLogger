﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace WebApp.Core.Extensions
{
    public static class EnumExtension
    {
        public static List<EnumProperty> GetKeyValues(Type enumType)
        {
            try
            {
                var data = new List<EnumProperty>();
                if (enumType.IsEnum)
                {
                    foreach (var e in enumType.GetEnumValues())
                    {
                        var val = GetDescription(enumType, e.ToString());
                        //data.Add(new EnumProperty { Id = Convert.ToInt32(e) , Value = val ?? e.ToString() });
                        data.Add(new EnumProperty
                        {
                            Id = Convert.ToInt32(e),
                            Value = val ?? string.Join(" ", Regex.Split(e.ToString(), @"([A-Z]?[a-z]+)").Where(str => !string.IsNullOrEmpty(str)))
                        });
                    }
                    return data;
                }

                return null;
            }
            catch (Exception e)
            {
                throw new Exception("Invalid Enum", e);
            }
        }

        public static string GetDescription(Type enumType, string name)
        {
            var filedName = enumType.GetFields().FirstOrDefault(x => x.Name == name);
            if (filedName != null)
            {
                var attr = Attribute.GetCustomAttribute(filedName, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attr != null)
                {
                    return attr.Description;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static string GetEnumDescription(this Enum value)
        {
            try
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());

                if (fi == null) return "Enum type not Found";

                DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

                if (attributes != null && attributes.Any())
                {
                    return attributes.First().Description;
                }

                //return value.ToString();
                return string.Join(" ", Regex.Split(value.ToString(), @"([A-Z]?[a-z]+)").Where(str => !string.IsNullOrEmpty(str)));
            }
            catch (Exception e)
            {
                return "Enum type not Found";
            }
        }

        //public static string GetEnumDescription(Enum value)
        //{
        //    try
        //    {
        //        FieldInfo fi = value.GetType().GetField(value.ToString());

        //        if (fi == null) return "Enum type not Found";

        //        DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

        //        if (attributes != null && attributes.Any())
        //        {
        //            return attributes.First().Description;
        //        }

        //        //return value.ToString();
        //        return string.Join(" ", Regex.Split(value.ToString(), @"([A-Z]?[a-z]+)").Where(str => !string.IsNullOrEmpty(str)));
        //    }
        //    catch (Exception e)
        //    {
        //        return "Enum type not Found";
        //    }
        //}

        /// <summary>
        /// Get enum int value by enum key
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="key">Key of the Enum</param>
        /// <returns>int value from enum</returns>
        public static int ParseEnum<T>(string key)
        {
            return (int)Enum.Parse(typeof(T), key, true);
        }

        public class EnumProperty
        {
            public int Id { get; set; }
            public string Value { get; set; }
        }

        public static IEnumerable<string> GetDescriptions(Type type)
        {
            var descs = new List<string>();
            var names = Enum.GetNames(type);
            foreach (var name in names)
            {
                var field = type.GetField(name);
                var fds = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                foreach (DescriptionAttribute fd in fds)
                {
                    descs.Add(fd.Description);
                }
            }
            return descs;
        }

        public static List<int> GetValuesFromDescription<T>(string description)
        {
            var Ids = new List<int>();
            var type = typeof(T);

            if (!type.IsEnum)
                throw new InvalidOperationException();

            foreach (var field in type.GetFields())
            {
                if (field.Name.ToLower() == "value__")
                    continue;

                var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description.ToLower().Contains(description.ToLower()))
                    {
                        var name = (T)field.GetValue(null);
                        var Id = Convert.ToInt32(name);
                        Ids.Add(Id);
                    }
                }
                else
                {
                    var fieldName = string.Join(" ", Regex.Split(field.Name.ToString(), @"([A-Z]?[a-z]+)").Where(str => !string.IsNullOrEmpty(str)));
                    if (fieldName.ToLower().Contains(description.ToLower()))
                    {
                        var name = (T)field.GetValue(null);
                        var Id = Convert.ToInt32(name);
                        Ids.Add(Id);
                    }
                }
            }
            return Ids;
        }
    }
}
