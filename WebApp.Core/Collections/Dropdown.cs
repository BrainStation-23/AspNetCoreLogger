using System.Collections.Generic;

namespace WebApp.Core.Collections
{
    public class Dropdown<T>
    {
        public IList<T> Data { get; set; }
        public int Size { get; set; }

        public Dropdown(IList<T> data, int size)
        {
            Data = data;
            Size = size;
        }

        public Dropdown()
        {
        }

        public Dropdown(IDropdown<T> list)
        {
            Data = list.Data;
        }
    }
}
