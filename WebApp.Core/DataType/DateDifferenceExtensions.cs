using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Core.DataType
{
    public class DateDifferenceExtensions
    {
        /// <summary>
        /// defining Number of days in month; index 0=> january and 11=> December
        /// february contain either 28 or 29 days, that's why here value is -1
        /// which wil be calculate later.
        /// </summary>
        private int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        // contain from date
        private DateTime fromDate;

        // contain To Date
        private DateTime toDate;

        // this three variable for output representation..
        private int year;
        private int month;
        private int day;

        public DateDifferenceExtensions(DateTime d1, DateTime d2)
        {
            if (d1 > d2)
            {
                fromDate = d2;
                toDate = d1;
            }
            else
            {
                fromDate = d1;
                toDate = d2;
            }

            // Day Calculation
            var increment = 0;

            if (fromDate.Day > toDate.Day)
            {
                increment = monthDay[fromDate.Month - 1];

            }
            // if it is february month
            // if it's to day is less then from day
            if (increment == -1)
            {
                increment = DateTime.IsLeapYear(fromDate.Year) ? 29 : 28;
            }
            if (increment != 0)
            {
                day = toDate.Day + increment - fromDate.Day;
                increment = 1;
            }
            else
            {
                day = toDate.Day - fromDate.Day;
            }

            // month calculation
            if (fromDate.Month + increment > toDate.Month)
            {
                month = toDate.Month + 12 - (fromDate.Month + increment);
                increment = 1;
            }
            else
            {
                month = toDate.Month - (fromDate.Month + increment);
                increment = 0;
            }

            // year calculation
            year = toDate.Year - (fromDate.Year + increment);

        }

        public override string ToString()
        {
            // return base.ToString();
            return year + " Year(s), " + month + " month(s), " + day + " day(s)";
        }

        public int Years
        {
            get
            {
                return year;
            }
        }

        public int Months
        {
            get
            {
                return month;
            }
        }

        public int Days
        {
            get
            {
                return day;
            }
        }
    }
}