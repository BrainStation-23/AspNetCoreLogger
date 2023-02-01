using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Core.Acls;
using WebApp.Logger.Interceptors;
using WebApp.Logger.Loggers.Repositories;

namespace WebApp.Logger.Loggers
{
    public class AuditLogIdentityUserContext<TUser, TKey, TUserClaim, TUserLogin, TUserToken> : IdentityUserContext<TUser, TKey, TUserClaim, TUserLogin, TUserToken>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserLogin : IdentityUserLogin<TKey>
        where TUserToken : IdentityUserToken<TKey>
    {
        public readonly ISignInHelper _signInHelper;
        private readonly ISqlLogRepository _sqlLogRepository;
        public readonly IHttpContextAccessor HttpContextAccessor;
        private ILoggerFactory _myLoggerFactory;
        private readonly IServiceProvider _serviceProvider;

        public AuditLogIdentityUserContext(DbContextOptions options,
            IServiceProvider serviceProvider) : base(options)
        {
            _serviceProvider = serviceProvider;
            _signInHelper = _serviceProvider.GetService<ISignInHelper>();
            HttpContextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();
            _sqlLogRepository = _serviceProvider.GetService<ISqlLogRepository>();
        }
        protected AuditLogIdentityUserContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
            optionsBuilder.LogTo(message => LoggerExtension.SqlQueryLog(message));
            optionsBuilder.AddInterceptors(new SqlQueryInterceptor(HttpContextAccessor, _sqlLogRepository));
            optionsBuilder.AddInterceptors(new SqlConnectionInterceptor(HttpContextAccessor, _sqlLogRepository));
            //optionsBuilder.AddInterceptors(new SqlSaveChangesInterceptor(HttpContextAccessor, _sqlLogRepository));
            optionsBuilder.AddInterceptors(new SqlTransactionInterceptor(HttpContextAccessor, _sqlLogRepository));
            optionsBuilder.UseLoggerFactory(_myLoggerFactory).EnableSensitiveDataLogging();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            bool hasAuditData = false;
            hasAuditData = await AuditTrailLog();

            if (hasAuditData)
                return await base.SaveChangesAsync(cancellationToken);

            return 0;
        }

        public override int SaveChanges()
        {
            //AuditTrailLog().ConfigureAwait(false);
            Task.Run(async () => AuditTrailLog());
            //AuditTrailLog().GetAwaiter().GetResult();

            return base.SaveChanges();
        }

        #region audit logic
        private async Task<bool> AuditTrailLog()
        {
            long userId = 0;

            if (_signInHelper.IsAuthenticated)
                userId = (long)_signInHelper.UserId;

            var auditEntries = base.ChangeTracker.AuditTrailLog(userId);
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
        #endregion
    }
}
