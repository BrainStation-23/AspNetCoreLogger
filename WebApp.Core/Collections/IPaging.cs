using System;
using System.Collections.Generic;

namespace WebApp.Core.Collections
{
    public class IPaging<T>
    {
        public IList<T> Data { get; set; }
        public int Total { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set; }
    }
}
