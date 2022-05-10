using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebApp.Core.DataType;

namespace WebApp.Core.DataType
{
    public static class DataSetExtetion
    {
        public static List<T> ToList<T>(this DataTable dataTable)
        {
            //var list = new List<T>();
            //var classType = typeof(T);
            //var classProperties = classType.GetProperties();
            //var dataColumns = dataTable.Columns.Cast<DataColumn>().ToList();

            //foreach (DataRow item in dataTable.Rows)
            //{
            //    var instance = (T)Activator.CreateInstance(classType);
            //    foreach (var pc in classProperties)
            //    {
            //        try
            //        {
            //            var d = dataColumns.Find(c => c.ColumnName == pc.Name);
            //            if (d != null)
            //                pc.SetValue(instance, item[pc.Name], null);
            //        }
            //        catch (Exception)
            //        {
            //            throw;
            //        }
            //    }

            //    list.Add(instance);
            //}

            //return list;

            var list = new List<T>();
            var classType = typeof(T);
            var classProperties = typeof(T).GetProperties().Select(propertyInfo => new
            {
                PropertyInfo = propertyInfo,
                Type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType
            }).ToList();

            foreach (var row in dataTable.Rows.Cast<DataRow>())
            {
                var instance = (T)Activator.CreateInstance(classType);

                foreach (var typeProperty in classProperties)
                {
                    try
                    {
                        var value = row[typeProperty.PropertyInfo.Name];
                        var safeValue = value == null || DBNull.Value.Equals(value)
                            ? null
                            : Convert.ChangeType(value, typeProperty.Type);

                        typeProperty.PropertyInfo.SetValue(instance, safeValue, null);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

                list.Add(instance);
            }

            return list;
        }
    }
}
