using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections.Generic;
using System.Diagnostics;
using WebApp.Common.Contexts;
using WebApp.Logger.Providers.Sqls;
using WebApp.Logger.Enums;
using WebApp.Logger.Loggers.Repositories;
using WebApp.Logger.Middlewares;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Mongos.Configurations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using WebApp.Logger.Endpoints;
using System;
using WebApp.Logger.Extensions;
using WebApp.Logger.Hostings;
using Microsoft.AspNetCore.Http;

namespace WebApp.Logger.Loggers
{
    public static class LoggerExtension
    {
        public static void AddDapper(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLoggerControllers();

            services.TryAddSingleton<DapperContext>(provider => new DapperContext(provider.GetService<IConfiguration>(), "WebAppConnection"));

            var logOptions = configuration.GetSection(LogOption.Name).Get<LogOption>();
       
            services.AddHostedService<RetentionPolicyService>();

            if (logOptions.ProviderType.ToString().ToLower() == "mssql")
            {
                services.AddScoped<IExceptionLogRepository, ExceptionLogRepository>();
                services.AddScoped<IRouteLogRepository, RouteLogRepository>();
                services.AddScoped<IAuditLogRepository, AuditLogRepository>();
                services.AddScoped<ISqlLogRepository, SqlLogRepository>();
            }
            if (logOptions.ProviderType.ToString().ToLower() == "mongo")
                services.AddMongoDb(configuration);

            if (logOptions.ProviderType.ToString().ToLower() == "cosmosdb")
                services.AddCosmosDb(configuration);

            if (logOptions.ProviderType.ToString().ToLower() == "file")
            {
                services.AddScoped<IExceptionLogRepository, ExceptionFileLogRepository>();
                services.AddScoped<IRouteLogRepository, RouteFileLogRepository>();
                services.AddScoped<IAuditLogRepository, AuditFileLogRepository>();
                services.AddScoped<ISqlLogRepository, SqlFileLogRepository>();
            }

            //services.AddMongoDb(configuration);
            //services.AddCosmosDb(configuration);
            


        }

        public static void AddLoggerControllers(this IServiceCollection services)
        {
            var assembly = typeof(LoggerWrapperController).Assembly;
            services.AddControllers()
                //.AddApplicationPart(Assembly.Load(new AssemblyName("ClassLibrary")));
                .AddApplicationPart(assembly);
        }

        //public static void AddLoggerEndpoints(this IEndpointRouteBuilder endpoints)
        //{
        //    endpoints.MapGet("/log/hello", async context =>
        //    {
        //        await context.Response.WriteAsync("Hello World!");
        //    });
        //}

        public static void AddHost()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseUrls("http://*:5002", "https://*:5003")
                        .UseStartup<LoggerWrapperStartup>();
                });

            host.Build().Run();
        }

        public static void HttpLog(this IApplicationBuilder app)
        {
            app.UseMiddleware<HttpRequestMiddleware>();
        }

        public static void ExceptionLog(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }

        //public static void HeaderLog(this IApplicationBuilder app)
        //{
        //    app.UseMiddleware<HttpRequestMiddleware>();
        //}

        /// <summary>
        /// Handling all changes in database
        /// WebAppDbContext] -> SaveChanges() -> base.ChangeTracker.AuditTrailLog(userId, nameof(AuditLog));;
        /// </summary>
        public static IList<AuditEntry> AuditTrailLog(this ChangeTracker changeTracker, long userId, string ignoreEntity = null)
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

        public static void AddLogConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<LogOption>(configuration.GetSection(LogOption.Name));
        }
    }
}
