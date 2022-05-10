//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using DataTables.Mvc;

//namespace WebApp.Core.DataType
//{
//    public static class DataTableExtention
//    {
//        public static IQueryable<T> Filter<T>(this IQueryable<T> source, IDataTablesRequest request, out int filteredTotal)
//        {
//            var sortedColumns = request.Columns.GetSortedColumns();

//            source = sortedColumns.Aggregate(source, (current, column) => current.OrderBy(column.Data, column.SortDirection == Column.OrderDirection.Ascendant));

//            filteredTotal = source.Count();

//            source = source.Skip(request.Start).Take(request.Length);
//            return source;
//        }
//    }
//}