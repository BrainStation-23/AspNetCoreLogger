using System.Collections.Generic;
using System.Linq;

namespace WebApp.Common.Serialize
{
    public static class DictionarySerializeExtentions
    {
        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            return obj.GetType()
                .GetProperties()
                .Select(pi => new { pi.Name, Value = pi.GetValue(obj, null) })
                .Union(
                    obj.GetType()
                        .GetFields()
                        .Select(fi => new { fi.Name, Value = fi.GetValue(obj) })
                )
                .ToDictionary(ks => ks.Name, vs => vs.Value);
        }
    }
}
