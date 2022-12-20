using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Common.Contexts;
using WebApp.Core.Acls;
using WebApp.Logger.Interceptors;
using WebApp.Logger.Loggers;
using WebApp.Logger.Loggers.Repositories;

namespace WebApp.Sql
{
    public class AuditLogContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken> : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserLogin : IdentityUserLogin<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
        where TUserToken : IdentityUserToken<TKey>
    {
        public readonly ISignInHelper _signInHelper;
        public readonly IAuditLogRepository _auditLogRepository;
        private readonly ISqlLogRepository _sqlLogRepository;
        public readonly IHttpContextAccessor HttpContextAccessor;
        private long? UserId;
        private ILoggerFactory _myLoggerFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly LogOption _logOptions;

        public AuditLogContext(DbContextOptions options,
            IConfiguration configuration,
            IServiceProvider serviceProvider) : base(options)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _signInHelper = _serviceProvider.GetService<ISignInHelper>();
            _auditLogRepository = _serviceProvider.GetService<IAuditLogRepository>();
            HttpContextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();
            UserId = _signInHelper.UserId;
            _sqlLogRepository = _serviceProvider.GetService<ISqlLogRepository>();
            //var logOption = new LogOption();
            _logOptions = configuration.GetSection(LogOption.Name).Get<LogOption>();
        }

        protected AuditLogContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
            optionsBuilder.LogTo(message => LoggerExtension.SqlQueryLog(message));
            optionsBuilder.AddInterceptors(new SqlQueryInterceptor(HttpContextAccessor, _sqlLogRepository));
            //optionsBuilder.AddInterceptors(new SqlSaveChangesInterceptor(HttpContextAccessor, _sqlLogRepository, _logOptions));
            //optionsBuilder.AddInterceptors(new SqlTransactionInterceptor(HttpContextAccessor, _sqlLogRepository, _logOptions));
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
            //if (!logOptions.LogType.MustContain(LogType.Sql.ToString()))
            //    skip = true;

            long userId = 0;

            if (_signInHelper.IsAuthenticated)
                userId = (long)_signInHelper.UserId;

            var auditEntries = base.ChangeTracker.AuditTrailLog(userId);
            auditEntries.ToList().ForEach(x => x.TraceId = HttpContextAccessor.HttpContext.TraceIdentifier);

            if (auditEntries.Any())
            {
                var factory = new ProviderFactory(_serviceProvider);

                var providerType = _logOptions.ProviderType;
                ILog loggerWrapper = factory.Build(providerType);

                await loggerWrapper.Audit.AddAsync(auditEntries.ToList());
            }

            return auditEntries.Any();
        }
        #endregion
    }
}
