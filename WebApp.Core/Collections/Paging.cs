using System;
using System.Collections.Generic;

namespace WebApp.Core.Collections
{
    public class Paging<T> : IPaging<T>// where T : class, new()
    {
        public Paging(int pageIndex, int pageSize, IList<T> data, int total)
        {
            if (pageSize < 1)
                throw new ArgumentException("Page Size must be greater than 0 for paginated data.");

            PageIndex = pageIndex;
            PageSize = pageSize;
            Total = total;
            Data = data;
            CurrentPage = total == 0 ? 0 : pageIndex + 1;
            TotalPage = (int)Math.Ceiling((Total / (double)PageSize));
            HasNextPage = CurrentPage < TotalPage;
            HasPreviousPage = CurrentPage > 1;
        }

        public Paging()
        {
        }

        public Paging(IList<T> data, int total)
        {
            Data = data;
            Total = total;
        }

        public Paging(IPaging<T> list)
        {
            Data = list.Data;
            Total = list.Total;
        }
    }
}
