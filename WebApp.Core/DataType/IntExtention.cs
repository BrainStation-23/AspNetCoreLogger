using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Core.DataType
{
    public static class IntExtention
    {
        public static int ToInt(this int? value)
        {
            int id;
            int.TryParse(value.ToString(), out id);

            return id;
        }

        /// <summary>
        /// Converts any type in to an Int32
        /// </summary>
        /// <typeparam name="T">Any Object</typeparam>
        /// <param name="value">Value to convert</param>
        /// <returns>The integer, 0 if unsuccessful</returns>
        public static int ToInt32<T>(this T value)
        {
            int result;
            if (int.TryParse(value.ToString(), out result))
            {
                return result;
            }
            return 0;
        }

        /// <summary>
        /// Converts any type in to an Int32 but if null then returns the default
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <typeparam name="T">Any Object</typeparam>
        /// <param name="defaultValue">Default to use</param>
        /// <returns>The defaultValue if unsuccessful ("a".ToInt32(100); // Returns 100 since a is nan)</returns>
        public static int ToInt32<T>(this T value, int defaultValue)
        {
            int result;
            if (int.TryParse(value.ToString(), out result))
            {
                return result;
            }
            return defaultValue;
        }
    }
}