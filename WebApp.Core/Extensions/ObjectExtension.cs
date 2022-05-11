using System.Collections.Generic;
using WebApp.Core.Models;
using System.Linq;
using WebApp.Core.Sqls;

namespace WebApp.Core.Extensions
{
    public static class ObjectExtension
    {
        public static IList<PropertyModel> GetPropertyInfo(this object o)
        {
            IList<PropertyModel> info = null;

            foreach (var propertyInfo in o.GetType().GetProperties())
            {
                if (propertyInfo.GetIndexParameters().Length == 0)
                {
                    info.Add(new PropertyModel(propertyInfo.Name, propertyInfo.GetValue(o), propertyInfo.GetType()));
                }
            }

            return info;
        }

        public static IList<string> GetPropertyNames(this object o)
        {
            return o.GetPropertyInfo().Select(e => e.Name).ToList();
        }
    }
}
