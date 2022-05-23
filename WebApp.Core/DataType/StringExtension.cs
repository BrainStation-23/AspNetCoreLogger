using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace WebApp.Core.DataType
{
    public static class StringExtension
    {
        public static string ToString(this object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), "Null value not allowed");

            return value.ToString();
        }

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

        public static bool IsNotNullOrWhiteSpace(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static List<string> ToSeperateList(this string value, char delimeter = ',')
        {
            return value.Split(delimeter).Select(e => e.Trim()).ToList();
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

        public static string ToSentence(this string input)
        {
            return new string(input.ToCharArray().SelectMany((c, i) => i > 0 && char.IsUpper(c) ? new[] { ' ', c } : new[] { c }).ToArray());
        }

        public static string SplitCamelCase(this string input)
        {
            //Regex.Replace(input, "([a-z](?=[A-Z]|[0-9])|[A-Z](?=[A-Z][a-z]|[0-9])|[0-9](?=[^0-9]))", "$1") //([A-Z]+(?=$|[A-Z][a-z]|[0-9])|[A-Z]?[a-z]+|[0-9]+)/g
            //Regex.Replace(input, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1")
            //Regex.Replace(input, "((?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z]))", " $1").Trim();
            //Regex.Replace(input, "(\\B[A-Z])", " $1")
            return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
        }

        public static string AddSpacesToSentence(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);

            for (var i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(text[i]);
            }

            return newText.ToString();
        }

        public static string StandardizeUrl(string url)
        {
            url = url.ToLower();

            if (!url.StartsWith("http://"))
            {
                url = string.Concat("http://", url);
            }

            return url;
        }
    }
}
