using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Core.Acls;
using WebApp.Logger.Interceptors;
using WebApp.Logger.Loggers.Repositories;

namespace WebApp.Logger.Loggers
{
    public class DbContextExtension : IDbContextExtension
    {
        private readonly IHttpContextAccessor HttpContextAccessor;
        private readonly ILoggerFactory _myLoggerFactory;
        private readonly ISqlLogRepository _sqlLogRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly ISignInHelper _signInHelper;
        public DbContextExtension(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            HttpContextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();
            _sqlLogRepository = _serviceProvider.GetService<ISqlLogRepository>();
            _myLoggerFactory = _serviceProvider.GetService<ILoggerFactory>();
            _signInHelper = _serviceProvider.GetService<ISignInHelper>();
        }

        public void Configuring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
            optionsBuilder.LogTo(message => LoggerExtension.SqlQueryLog(message));
            optionsBuilder.AddInterceptors(new SqlQueryInterceptor(HttpContextAccessor, _sqlLogRepository));
            optionsBuilder.AddInterceptors(new SqlConnectionInterceptor(HttpContextAccessor, _sqlLogRepository));
            //optionsBuilder.AddInterceptors(new SqlSaveChangesInterceptor(HttpContextAccessor, _sqlLogRepository));
            optionsBuilder.AddInterceptors(new SqlTransactionInterceptor(HttpContextAccessor, _sqlLogRepository));
            optionsBuilder.UseLoggerFactory(_myLoggerFactory).EnableSensitiveDataLogging();
        }

        public async Task<bool> AuditTrailLogAsync(DbContext context)
        {
            long userId = 0;

            if (_signInHelper.IsAuthenticated)
                userId = (long)_signInHelper.UserId;
            var auditEntries = context.ChangeTracker.AuditTrailLog(userId);
            auditEntries.ToList().ForEach(x => x.TraceId = HttpContextAccessor.HttpContext.TraceIdentifier);
            auditEntries.ToList().ForEach(x => x.ApplicationName = AppDomain.CurrentDomain.FriendlyName.ToString());
            auditEntries.ToList().ForEach(x => x.ControllerName = HttpContextAccessor.HttpContext.Request.RouteValues["controller"].ToString());
            auditEntries.ToList().ForEach(x => x.ActionName = HttpContextAccessor.HttpContext.Request.RouteValues["action"].ToString());

            if (auditEntries.Any())
            {
                await BatchLoggingContext.PublishAsync(auditEntries.ToList(), LogType.Audit.ToString());
            }

            return auditEntries.Any();
        }

    }
}
