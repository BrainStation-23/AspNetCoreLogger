using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Common.Contexts;
using WebApp.Core.Acls;
using WebApp.Core.Contexts;
using WebApp.Entity.Entities.Blogs;
using WebApp.Entity.Entities.Identities;
using WebApp.Entity.Entities.Logs;
using WebApp.Entity.Entities.Settings;
using WebApp.Logger.Interceptors;
using WebApp.Logger.Loggers;
using static WebApp.Entity.Entities.Identities.IdentityModel;

namespace WebApp.Sql
{
    public class WebAppContext : IdentityDbContext<User,
        Role, long,
        UserClaim,
        UserRole,
        UserLogin,
        RoleClaim,
        UserToken>
    {
        public const string DefaultSchemaName = "dbo";
        public string Schema { get; set; }

        public readonly ISignInHelper SignInHelper;
        public readonly IHttpContextAccessor HttpContextAccessor;

        public WebAppContext(DbContextOptions<WebAppContext> options,
            ISignInHelper signInHelper,
            IHttpContextAccessor httpContextAccessor) : base(options)
        {
            SignInHelper = signInHelper;
            HttpContextAccessor = httpContextAccessor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
            optionsBuilder.LogTo(message => LoggerExtension.SqlQueryLog(message));
            //optionsBuilder.AddInterceptors(new SqlQueryInterceptor(HttpContextAccessor));
            optionsBuilder.UseLoggerFactory(_myLoggerFactory).EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema(Schema);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.RelationConvetion();
            builder.DecimalConvention();
            builder.DateTimeConvention();
        }

        public static readonly LoggerFactory _myLoggerFactory = new LoggerFactory(new[] {
            new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
                //new ConsoleLoggerProvider((_, __) => true, true)
        });

        #region Audit Logic
        private void Audit()
        {
            long userId = 0;


            if (SignInHelper.IsAuthenticated)
                userId = (long)SignInHelper.UserId;

            base.ChangeTracker.Audit(userId);
        }

        private bool AuditTrailLog()
        {
            long userId = 0;

            if (SignInHelper.IsAuthenticated)
                userId = (long)SignInHelper.UserId;

            var auditEntries = base.ChangeTracker.AuditTrailLog(userId, nameof(AuditLog));

            if (auditEntries.Any())
            {
                foreach (var auditEntry in auditEntries)
                {
                    var audit = new AuditLog
                    {
                        UserId = auditEntry.UserId,
                        Type = auditEntry.AuditType.ToString(),
                        TableName = auditEntry.TableName,
                        DateTime = DateTime.Now,
                        PrimaryKey = JsonConvert.SerializeObject(auditEntry.KeyValues),
                        OldValues = auditEntry.OldValues.Count == 0 ? null : JsonConvert.SerializeObject(auditEntry.OldValues),
                        NewValues = auditEntry.NewValues.Count == 0 ? null : JsonConvert.SerializeObject(auditEntry.NewValues),
                        AffectedColumns = auditEntry.ChangedColumnNames.Count == 0 ? null : JsonConvert.SerializeObject(auditEntry.ChangedColumnNames)
                    };
                    //var data = auditEntry.Changes.Count == 0 ? null : JsonConvert.SerializeObject(auditEntry.Changes);

                    AuditLogs.Add(audit);
                }
            }

            return auditEntries.Any();
        }
        #endregion

        #region acl entitites
        //public DbSet<IdentityRole<long>> AspNetRoles { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<RoleMenuMapper> RoleMenuMappers { get; set; }
        #endregion

        #region settings
        public DbSet<Setting> Settings { get; set; }
        #endregion

        #region logs
        public DbSet<SmsLog> SmsLogs { get; set; }
        public DbSet<EmailLog> EmailLogs { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<RequestLog> RequestLogs { get; set; }
        #endregion

        public DbSet<BlogEntity> Blogs { get; set; }
        public DbSet<PostEntity> Posts { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            Audit();
            var hasAuditData = AuditTrailLog();
            if (hasAuditData)
                return base.SaveChangesAsync(cancellationToken);

            return Task.FromResult(0);
        }
        public override int SaveChanges()
        {
            Audit();
            AuditTrailLog();

            return base.SaveChanges();
        }
    }
}
