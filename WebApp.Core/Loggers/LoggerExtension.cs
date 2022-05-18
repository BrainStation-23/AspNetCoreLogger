using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using WebApp.Core.Contexts;
using WebApp.Core.Enums;
using WebApp.Core.Middlewares;
using WebApp.Core.Models;

namespace WebApp.Core.Loggers
{
    public static class LoggerExtension
    {
        /// <summary>
        /// Handling all exception message globaly with custom middleware
        /// startup.cs -> app.ExceptionLogCustom();
        /// </summary>
        public static void ExceptionLog(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalExceptionCustomMiddleware>();
        }

        /// <summary>
        /// Handling all exception message globaly with builtin middleware
        /// startup.cs -> app.ExceptionLog(log);
        /// </summary>
        public static void ExceptionLog(this IApplicationBuilder app, ILogger logger)
        {
            app.ConfigureExceptionHandler(logger);
        }

        /// <summary>
        /// Handling all changes in database
        /// WebAppDbContext] -> SaveChanges() -> base.ChangeTracker.AuditTrailLog(userId, nameof(AuditLog));;
        /// </summary>
        public static IList<AuditEntry> AuditTrailLog(this ChangeTracker changeTracker, long userId, string ignoreEntity)
        {
            return changeTracker.AuditTrail(userId, ignoreEntity);
        }

        /// <summary>
        /// All db generated sql queries log
        /// WebAppDbContext.cs -> OnConfiguring(DbContextOptionsBuilder optionsBuilder) -> optionsBuilder.LogTo(message => LoggerExtension.SqlQueryLog(message));;
        /// </summary>
        public static void SqlQueryLog(string query, LogStoreType storeType = LogStoreType.Output)
        {
            if (storeType == LogStoreType.Output)
            {
                Debug.WriteLine(query);
            }
            else if (storeType == LogStoreType.Db)
            {
                // store in db
            }
            else if (storeType == LogStoreType.File)
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
