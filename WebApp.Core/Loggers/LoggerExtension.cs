using System.Diagnostics;

namespace WebApp.Core.Loggers
{
    public static class LoggerExtension
    {
        public static void WriteSqlQueryLog(string query, LogStoreTypeEnum storeType = LogStoreTypeEnum.Output)
        {
            if (storeType == LogStoreTypeEnum.Output)
                Debug.WriteLine(query);
            else if (storeType == LogStoreTypeEnum.Db)
            {
                // store in db
            }
            else if (storeType == LogStoreTypeEnum.File)
            {
                // store & append in file
                //new StreamWriter("mylog.txt", append: true);
            }

            //using (WebAppContext context = new WebAppContext())
            //{
            //    context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
            //}

            //IQueryable queryable = from x in Blogs
            //                       where x.Id == 1
            //                       select x;

            //var sqlEf5 = ((System.Data.Objects.ObjectQuery)queryable).ToTraceString();
            //var sqlEf6 = ((System.Data.Entity.Core.Objects.ObjectQuery)queryable).ToTraceString();
            //var sql = queryable.ToString();

            // https://docs.microsoft.com/en-us/ef/ef6/fundamentals/logging-and-interception?redirectedfrom=MSDN
            // https://stackoverflow.com/questions/1412863/how-do-i-view-the-sql-generated-by-the-entity-framework
        }
    }
}
