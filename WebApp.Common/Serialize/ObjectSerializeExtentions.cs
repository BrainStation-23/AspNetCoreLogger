
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Common.Serialize
{
    public static class ObjectSerializeExtentions
    {
        //public static List<string> typecontainer = new List<string>();


        //public static object ReadNestedObject(this object obj)
        //{
        //    if (obj == null)
        //        return null;

        //    var typeName = obj.GetType().Name.ToLower();

        //    //if (!HashTableSerializeExtentions.typecontainer.Contains(typeName))
        //    //{
        //    //    HashTableSerializeExtentions.typecontainer.Add(typeName);
        //    //}

        //    if (typeName.Contains("object") || typeName.Contains("dictionary"))
        //    {
        //        var hash = obj.ToJson().ToModel<Hashtable>();
        //        var newHash = new Hashtable();

        //        foreach (var key in hash.Keys)
        //        {
        //            var newHashValue = ReadNestedObject(hash[key]);
        //            newHash.Add(key, newHashValue);
        //        }

        //        return newHash;
        //    }
        //    else if (typeName.Contains("list") || typeName.Contains("array"))
        //    {
        //        var list = obj.ToJson().ToModel<List<object>>();

        //        if (list.Count == 0)
        //            return null;

        //        var newlist = new List<object>();

        //        foreach (var val in list)
        //        {
        //            newlist.Add(ReadNestedObject(val));
        //        }

        //        return newlist;
        //    }
        //    else
        //    {
        //        return obj;
        //    }
        //}
    }
}
