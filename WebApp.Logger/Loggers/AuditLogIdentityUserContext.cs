using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebApp.Logger.Loggers
{
    public class AuditLogIdentityUserContext<TUser, TKey, TUserClaim, TUserLogin, TUserToken> : IdentityUserContext<TUser, TKey, TUserClaim, TUserLogin, TUserToken>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserLogin : IdentityUserLogin<TKey>
        where TUserToken : IdentityUserToken<TKey>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDbContextExtension _dbContextExtension;

        public AuditLogIdentityUserContext(DbContextOptions options,
            IServiceProvider serviceProvider) : base(options)
        {
            _serviceProvider = serviceProvider;
            _dbContextExtension = _serviceProvider.GetService<IDbContextExtension>();
        }
        protected AuditLogIdentityUserContext() { }

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
            Task.Run(AuditTrailLog);
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
