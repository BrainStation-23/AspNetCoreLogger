using System.Collections.Generic;

namespace WebApp.Common.Collections
{
    public class PagedList<T> : IPagedList<T> where T : class
    {
        public IList<T> Items { get; set; }
        public int Total { get; set; }

        public PagedList()
        {

        }

        public PagedList(IList<T> items, int total)
        {
            Items = items;
            Total = total;
        }

        public PagedList(IPagedList<T> list)
        {
            Items = list.Items;
            Total = list.Total;
        }
    }
}
