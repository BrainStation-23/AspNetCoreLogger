using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Core.DataType
{
    public static class DoubleExtention
    {
        /// <summary>
        /// Return currenncy for specific  culture
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureName"></param>
        /// <returns> (154.20).ToCurrency("en-US") returns $154.20</returns>
        public static string ToCurrency(this double value, string cultureName = "en-US")
        {
            CultureInfo currentCulture = new CultureInfo(cultureName);
            return (string.Format(currentCulture, "{0:C}", value));
        }
    }
}
