using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Core.DataType
{
    public static class Pagination
    {
        /// <summary>
        /// get query after Skip and Take from IQueryable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="page">Page Number</param>
        /// <param name="pageSize">Page Size for Per Page </param>
        /// <returns></returns>
        public static IQueryable<T> Paginate<T>(this IQueryable<T> value, int page = 1, int pageSize = 10)
        {
            return value.Skip((page - 1) * pageSize).Take(pageSize).AsQueryable();
        }
    }
}