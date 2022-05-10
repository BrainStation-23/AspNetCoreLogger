using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebApp.Core.DataType;

namespace WebApp.Core.DataType
{
    public static class DateExtention
    {
        public enum MonthType
        {
            All = 0,
            Previous,
            Next
        }

        public static DateTime? ToDateTime(this string dateString)
        {
            var dateParse = DateTime.TryParse(dateString, out DateTime date);
            return dateParse ? date : new DateTime?();
        }
        public static DateTime ConvertStringToDateTime(this string dateString, string dateFormat)
        {
            try
            {
                var date = new DateTime();
                if (!string.IsNullOrEmpty(dateString) && !string.IsNullOrEmpty(dateFormat))
                    date = DateTime.ParseExact(dateString, dateFormat, CultureInfo.InvariantCulture);
                return date;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public static string ToNullableShortDateString(this DateTime? date)
        {
            return date.HasValue ? DateTime.Parse(date.ToString()).ToShortDateString() : string.Empty;
        }
        public static string ToNullableLongDateString(this DateTime? date)
        {
            return date.HasValue ? DateTime.Parse(date.ToString()).ToLongDateString() : string.Empty;
        }
        public static string ToNullableShortDateStringOrNull(this DateTime? date)
        {
            return date.HasValue ? DateTime.Parse(date.ToString()).ToShortDateString() : null;
        }

        /// <summary>
        /// Get months list
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, int> GetMonths(MonthType? monthType = MonthType.All)
        {
            var monthArray = DateTimeFormatInfo.InvariantInfo.MonthNames;

            Dictionary<string, int> months;

            switch (monthType)
            {
                case MonthType.Previous:
                    months = monthArray
                        .Where(m => Array.IndexOf(monthArray, m) + 1 <= DateTime.Now.Month)
                        .Select((value, index) => new { value, index })
                        .ToDictionary(pair => pair.value, pair => pair.index + 1);
                    break;
                case MonthType.Next:
                    months = monthArray
                        .Where(m => Array.IndexOf(monthArray, m) + 1 >= DateTime.Now.Month)
                        .Select((value, index) => new { value, index })
                        .ToDictionary(pair => pair.value, pair => pair.index + 1);
                    break;
                default:
                    months = monthArray
                        .Select((value, index) => new { value, index })
                        .ToDictionary(pair => pair.value, pair => pair.index + 1);
                    break;
            }

            return months;
        }

        public static string ToMonthName(int monthNumber)
        {
            var currentDate = DateTime.Now;
            var tempDate = new DateTime(currentDate.Year, monthNumber, currentDate.Day);
            return tempDate.ToString("MMMM");
        }

        /// <summary>
        /// Get motnhs dropdown 
        /// </summary>
        /// <param name="selected"></param>
        /// <param name="previous"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetMonthsDropdown(int? selected = 0, MonthType? monthType = MonthType.All)
        {
            var months = GetMonths(monthType);

            return months.Select(e => new SelectListItem
            {
                Text = e.Key,
                Value = e.Value.ToString(),
                Selected = Convert.ToInt32(e.Value + 1) == selected
            });
        }

        /// <summary>
        /// Get years list
        /// </summary>
        /// <returns></returns>
        public static List<string> GetYears(int previous = 10, int next = 0)
        {
            var date = DateTime.Now;
            date = date.AddYears(-previous);

            var years = Enumerable.Range(date.Year, previous + next + 1)
                .Select(i => i.ToString())
                .OrderByDescending(i => i)
                .ToList();

            return years;
        }

        /// <summary>
        /// Get years dropdown 
        /// </summary>
        /// <param name="selected"></param>
        /// <param name="previous"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetYearsDropdown(int? selected = 0, int previous = 10, int next = 0)
        {
            var years = GetYears(previous, next);

            return years.Select(e => new SelectListItem
            {
                Text = e,
                Value = e,
                Selected = Convert.ToInt16(e) == selected
            });
        }
    }
}