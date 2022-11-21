using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WebApp.Logger.Loggers;

namespace WebApp.Logger.Extensions
{
    public static class EnumExtension
    {
        /// <summary>
        /// string value to enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">string value which is same as enum name</param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value, T defaultValue)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static List<ProviderType> ToProviderTypeEnums(this List<string> stringList)
        {
            //var d = stringList
            //      .Select(x => (ProviderType)Enum.Parse(typeof(ProviderType), x, true))
            //      .ToList();

            var d = stringList
                  .Select(x => x.ToEnum(ProviderType.MSSql))
                  .Distinct()
                  .ToList();

            return d;
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static List<string> EnumList<T>()
        {
            return Enum.GetNames(typeof(T)).ToList();
        }

        public static bool IsValidRetention(this string value)
        {
            var expression = @"^\d{1,2}[dmyHMS]{1}$";
            Regex regex = new Regex(expression);

            return regex.IsMatch(value);
        }
    }
}
