using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WebApp.Common.DataType
{
    public static class StringExtension
    {
        public static string ToNullString(this object value)
        {
            if (value == null)
                return string.Empty;

            return value.ToString();
        }

        public static bool IsNotNullOrEmpty(this string value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static string ToConcat(this IEnumerable<char> values)
        {
            return string.Concat(values);
        }

        public static string ToShorten(this string value)
        {
            var letters = new Regex("([A-Z]+[^A-Z]+)").Matches(value)
                             .Cast<Match>()
                             .Select(match => match.Value[0]);

            return letters.ToConcat();
        }
    }
}
