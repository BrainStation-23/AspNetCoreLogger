using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Logger.Loggers;

namespace WebApp.Logger.Extensions
{
    public static class EnumExtension
    {
        public static T ToEnum<T>(this string value, T defaultValue)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static List<ProviderType> ToProviderTypeEnums(this List<string> stringList)
        {
            //var d = stringList
            //      .Select(x => (ProviderType)Enum.Parse(typeof(ProviderType), x, true))
            //      .ToList();

            var d = stringList
                  .Select(x => x.ToEnum(ProviderType.MSSql))
                  .Distinct()
                  .ToList();

            return d;
        }
    }
}
