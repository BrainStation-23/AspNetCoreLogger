using System.Collections.Generic;

namespace WebApp.Core.Collections
{
    public interface IDropdown<T>
    {
        public IList<T> Data { get; set; }
    }
}
