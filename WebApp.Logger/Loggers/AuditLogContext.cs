using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Logger.Loggers;

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
        private readonly IServiceProvider _serviceProvider;
        private readonly IDbContextExtension _dbContextExtension;

        public AuditLogContext(DbContextOptions options,
            IServiceProvider serviceProvider) : base(options)
        {
            _serviceProvider = serviceProvider;
            _dbContextExtension = _serviceProvider.GetService<IDbContextExtension>();
        }

        protected AuditLogContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _dbContextExtension.Configuring(optionsBuilder);
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
            return await _dbContextExtension.AuditTrailLogAsync(this);
        }
        #endregion
    }
}
