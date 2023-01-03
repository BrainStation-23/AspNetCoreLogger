
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Common.Serialize
{
    public static class JObjectSerializeExtentions
    {
        //public static List<string> typecontainer = new List<string>();
        

        public static object IdentifyNestedObjects(this object obj)
        {
            if (obj == null)
                return null;

            var typeName = obj.GetType().Name.ToLower();

            //if (!HashTableSerializeExtentions.typecontainer.Contains(typeName))
            //{
            //    HashTableSerializeExtentions.typecontainer.Add(typeName);
            //}

            if (typeName.Contains("object")|| typeName.Contains("dictionary"))
            {
                return obj.ObjectToHashtable();
            }
            else if (typeName.Contains("list")|| typeName.Contains("array"))
            {
                return obj.ToJson().ToModel<List<object>>().ListToHashtable();
            }
            else
            {
                return obj;
            }
        }

        public static List<object> ListToHashtable(this List<object> list)
        {
            if (list == null)
                return null;

            if (list.Count == 0)
                return null;

            var newlist = new List<object>();

            foreach (var val in list)
            {
                newlist.Add(IdentifyNestedObjects(val));
            }
            return newlist;
        }

        public static Hashtable ObjectToHashtable(this object obj)
        {
            var hash = obj.ToJson().ToModel<Hashtable>();
            var newHash = new Hashtable();
            foreach (var key in hash.Keys)
            {
                var newHashValue = IdentifyNestedObjects(hash[key]);
                newHash.Add(key, newHashValue);
            }
            return newHash;
        }

    }
}
