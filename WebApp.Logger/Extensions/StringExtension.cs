using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WebApp.Logger.Extensions
{
    public static class StringExtension
    {
        public static bool IsNotNullOrEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        public static bool IsNotNullOrWhiteSpace(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        public static string ToShorten(this string value)
        {
            var letters = new Regex("([A-Z]+[^A-Z]+)").Matches(value)
                             .Cast<Match>()
                             .Select(match => match.Value[0]);

            return letters.ToConcat();
        }

        public static string ToConcat(this IEnumerable<char> values)
        {
            return string.Concat(values);
        }
    }
}
