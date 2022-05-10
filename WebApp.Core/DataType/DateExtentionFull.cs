using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebApp.Core.DataType;

namespace WebApp.Core.DataType
{
    public enum MonthType
    {
        Full,
        Next,
        Previous
    }

    public static class DateExtentionFull
    {
        public static bool IsValid(this DateTime? date)
        {
            return true;
        }

        public static bool IsValid(this DateTime date)
        {
            return true;
        }

        public static DateTime? ToDateTime(this string dateString)
        {

            var dateParse = DateTime.TryParse(dateString, out DateTime date);
            return dateParse ? date : new DateTime?();
        }

        /// <summary>
        /// Get names
        /// </summary>
        /// <returns></returns>
        public static List<string> GetWeeks()
        {
            return new List<string>();
        }

        /// <summary>
        /// get months between two date
        /// </summary>
        public static List<string> GetMonths(DateTime startDate, DateTime endDate)
        {
            return new List<string>();
        }

        /// <summary>
        /// get total days between two date
        /// </summary>
        public static int GetTotalDays(DateTime startDate, DateTime endDate)
        {
            return GetMonths(startDate, endDate).Count;
        }

        public static bool IsWeekend(this DateTime value)
        {
            return value.DayOfWeek == DayOfWeek.Sunday || value.DayOfWeek == DayOfWeek.Saturday;
        }

        /// <summary>
        /// get total months between two date
        /// </summary>
        public static int GetTotalMonths(DateTime startDate, DateTime endDate)
        {
            return GetMonths(startDate, endDate).Count;
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
        /// Get the list of months as numbers
        /// </summary>
        /// <returns></returns>
        public static List<int> GetMonthNumbers()
        {
            var date = DateTime.Now;
            var beforeMonths = Enumerable.Range(1, 12)
                .Select(i => new DateTime(date.Year, i, 1).Month)
                .ToList();

            //ToString(CultureInfo.InvariantCulture)
            return beforeMonths;
        }


        /// <summary>
        /// Get months list
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetMonths(MonthType type = MonthType.Full)
        {
            var date = DateTime.Now;
            var monthNumber = Enumerable.Range(1, 12);

            switch (type)
            {
                case MonthType.Previous:
                    monthNumber = monthNumber.Where(i => i <= date.Month);
                    break;
                case MonthType.Next:
                    monthNumber = monthNumber.Where(i => i >= date.Month);
                    break;
            }

            var months = monthNumber.Select(i => new DateTime(date.Year, i, 1).ToString("MMMM"))
                .ToList();

            return months;
        }

        /// <summary>
        /// Get months list
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetMonthsDropdown(int? selected = 0, MonthType type = MonthType.Full)
        {
            var months = GetMonths(type);

            return months.Select((v, i) => new SelectListItem
            {
                Text = v,
                Value = (i + 1).ToString(),
                Selected = i + 1 == selected
            });
        }

        /// <summary>
        /// Get the month name
        /// </summary>
        /// <param name="monthNumber"></param>
        /// <returns></returns>
        public static string GetMonthName(int monthNumber)
        {
            // dirty way
            var currentDate = DateTime.Now;
            var temDate = new DateTime(currentDate.Year, monthNumber, currentDate.Day);
            return temDate.ToString("MMMM");
        }

        /// <summary>
        /// Gets the month name
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetMonthName(this DateTime? date)
        {
            return date?.ToString("MMMM") ?? string.Empty;
        }

        /// <summary>
        /// Gets first day of the month.
        /// </summary>
        /// <returns></returns>
        public static DateTime? GetFirstDayOfMonth(this DateTime? date)
        {
            if (date == null)
                return null;

            return new DateTime(date.Value.Year, date.Value.Month, 1);
        }

        /// <summary>
        /// Get last day of the month.
        /// </summary>
        /// <returns></returns>
        public static DateTime? GetLastDayOfMonth(this DateTime? date)
        {
            if (date == null)
                return null;

            date = date.Value.AddMonths(1);
            return date.Value.AddDays(-date.Value.Day);
        }

        /// <summary>
        /// Get months list
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