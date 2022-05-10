using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Core.DataType
{
    public static class DateTimeExtension
    {
        public static long UnixTimestampConversion(DateTime dateTime)
        {
            long unixTime = (long)(TimeZoneInfo.ConvertTimeToUtc(DateTime.UtcNow)
                           - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            return unixTime;
        }

        public static DateTime ToLocalDateTime(this DateTime dateTime, TimeZoneInfo timeZoneInfo = null)
        {
            if (timeZoneInfo == null)
                timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");

            var local = TimeZoneInfo.ConvertTimeFromUtc(dateTime, timeZoneInfo);

            return local;
        }

        public static DateTime ToLocalDate(this DateTime dateTime, TimeZoneInfo timeZoneInfo = null)
        {
            var date = dateTime.ToLocalDateTime();
            return date.Date;
        }

        public static TimeSpan ToLocalTimes(this DateTime dateTime, TimeZoneInfo timeZoneInfo = null)
        {
            var date = dateTime.ToLocalDateTime();

            return date.TimeOfDay;
        }

        public static string ToFormattedDateTime(this DateTime dateAndTime, bool includeTime)
        {
            // Format: January 26th, 2006 at 2:19pm
            string dateSuffix = "<sup>th</sup>";
            switch (dateAndTime.Day)
            {
                case 1:
                case 21:
                case 31:
                    dateSuffix = "<sup>st</sup>";
                    break;
                case 2:
                case 22:
                    dateSuffix = "<sup>nd</sup>";
                    break;
                case 3:
                case 23:
                    dateSuffix = "<sup>rd</sup>";
                    break;
            }
            var dateFmt = string.Format("{0:MMMM} {1:%d}{2}, {3:yyyy} at {4:%h}:{5:mm}{6}",
                dateAndTime, dateAndTime, dateSuffix, dateAndTime, dateAndTime, dateAndTime,
                dateAndTime.ToString("tt").ToLower());
            if (!includeTime)
            {
                dateFmt = string.Format("{0:MMMM} {1:%d}{2}, {3:yyyy}",
                    dateAndTime, dateAndTime, dateSuffix, dateAndTime);
            }
            return dateFmt;
        }
        //Usage:
        //DateTime.Now.ToFormattedDateTime(false) // Returns: December 19th, 2018
        //DateTime.Now.ToFormattedDateTime(true) // Returns: December 19th, 2018 at 9:00a

        public static string ToW3CDate(this DateTime dt)
        {
            return dt.ToUniversalTime().ToString("s") + "Z";
        }
        //Usage:
        //DateTime.Now.ToW3CDate(); // Returns: 1997-07-16T19:20:30.45Z

        public static string ToDaysTil(this DateTime value, DateTime endDateTime)
        {
            var ts = new TimeSpan(endDateTime.Ticks - value.Ticks);
            var delta = ts.TotalSeconds;
            if (delta < 60)
            {
                return ts.Seconds == 1 ? "one second" : ts.Seconds + " seconds";
            }
            if (delta < 120)
            {
                return "a minute";
            }
            if (delta < 2700) // 45 * 60
            {
                return ts.Minutes + " minutes";
            }
            if (delta < 5400) // 90 * 60
            {
                return "an hour";
            }
            if (delta < 86400) // 24 * 60 * 60
            {
                return ts.Hours + " hours";
            }
            if (delta < 172800) // 48 * 60 * 60
            {
                return "yesterday";
            }
            if (delta < 2592000) // 30 * 24 * 60 * 60
            {
                return ts.Days + " days";
            }
            if (delta < 31104000) // 12 * 30 * 24 * 60 * 60
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month" : months + " months";
            }
            var years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "one year" : years + " years";
        }
        //Usage:
        //var christmas = new DateTime(2018, 12, 25);
        //var almost = DateTime.Now.ToDaysTil(christmas); // returns: "5 days" (you add what you want after it).


        /// <summary>
        /// This extension method places a 'th', 'st', 'nd', 'rd', or 'th' to the end of the number.
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string OrdinalSuffix(this DateTime datetime)
        {
            int day = datetime.Day;
            if (day % 100 >= 11 && day % 100 <= 13)
                return string.Concat(day, "th");
            switch (day % 10)
            {
                case 1:
                    return string.Concat(day, "st");
                case 2:
                    return string.Concat(day, "nd");
                case 3:
                    return string.Concat(day, "rd");
                default:
                    return string.Concat(day, "th");
            }
        }

        public static string DefaultFormat = "dd/MM/yyyy";

        public static bool IsGreaterThan(this DateTime dt1, DateTime dt2)
        {
            return dt1 > dt2;
        }

        public static bool IsLessThan(this DateTime dt1, DateTime dt2)
        {
            return dt1 < dt2;
        }
    }
}
