using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Data;
using System.Linq;
using WebApp.Common.Sqls;

namespace WebApp.Common
{
    public static class DbContextExtensions
    {
        public static void Audit(this ChangeTracker changeTracker, long userId)
        {
            var now = DateTimeOffset.UtcNow;

            foreach (var entry in changeTracker.Entries<BaseEntity>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                }

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = entry.Entity.CreatedBy != 0 ? entry.Entity.CreatedBy : userId;
                    entry.Entity.CreatedDateUtc = entry.Entity.CreatedDateUtc == DateTimeOffset.MinValue ? now : entry.Entity.CreatedDateUtc;
                    entry.Entity.UpdatedBy = 0;
                    entry.Entity.UpdatedDateUtc = null;
                }
                else
                {
                    entry.Entity.UpdatedDateUtc ??= now;
                    entry.Entity.UpdatedBy ??= userId;
                }
            }
        }

    }
}
