using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.Core.DataType;

namespace WebApp.Core.DataType
{
    public static class CollectionExtention
    {
        public static bool List()
        {
            var list = new List<string>();

            var d = list.ToDictionary(x => x, x => x);
            //list.ToDictionary(x => x, x => string.Format("Val: {0}", x));
            //list.Distinct().ToDictionary(x => x, x => x); 
            //list.ToLookup(x => x);    # distict
            //Dictionary<string, List<object>> dict = list
            //   .GroupBy(x => x.Id, x => x.Name)
            //   .ToDictionary(x => x.Key, x => x.ToList());

            return true;
        }

        public static bool In<T>(this T source, params T[] list)
        {
            if (null == source) throw new ArgumentNullException("source");
            return list.Contains(source);
        }

        /// <summary>
        /// 6.InRange(3,7))
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actual"></param>
        /// <param name="lower"></param>
        /// <param name="upper"></param>
        /// <returns></returns>
        public static bool InRange<T>(this T actual, T lower, T upper) where T : IComparable<T>
        {
            return actual.CompareTo(lower) >= 0 && actual.CompareTo(upper) < 0;
        }
    }
}