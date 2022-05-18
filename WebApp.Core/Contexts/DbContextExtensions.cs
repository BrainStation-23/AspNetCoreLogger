using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using WebApp.Core.Enums;
using WebApp.Core.Models;
using WebApp.Core.Sqls;

namespace WebApp.Core.Contexts
{
    public static class DbContextExtensions
    {
        public static IList<AuditEntry> AuditTrail(this ChangeTracker changeTracker, long userId, string ignoreEntity)
        {
            changeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in changeTracker.Entries())
            {
                if (entry.State == EntityState.Detached
                    || entry.State == EntityState.Unchanged
                    //|| entry.Entity is BaseEntity
                    //|| entry.Entity is AuditLog
                    || entry.Entity.GetType().Name == ignoreEntity)
                    continue;

                var auditEntry = new AuditEntry(entry)
                {
                    TableName = entry.Entity.GetType().Name,
                    UserId = userId
                };
                auditEntries.Add(auditEntry);
                var originalEntry = entry.GetDatabaseValues();
                var ignorePropertyName = typeof(BaseEntity).GetProperties().Select(e => e.Name).ToList();

                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;
                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;
                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = originalEntry[propertyName];
                                auditEntry.NewValues[propertyName] = property.CurrentValue;

                                if (ignorePropertyName.Contains(propertyName))
                                    continue;

                                var currentValue = property.CurrentValue?.ToString();
                                var originalValue = originalEntry[propertyName]?.ToString();
                                if (currentValue != originalValue)
                                {
                                    auditEntry.ChangedColumnNames.Add(propertyName);
                                    auditEntry.Changes[propertyName] = currentValue;
                                }
                            }
                            break;
                    }
                }
            }

            var data = changeTracker.DebugView.LongView;
            Debug.WriteLine(data);

            return auditEntries;
        }

        public static void Audit(this ChangeTracker changeTracker, long userId)
        {
            var now = DateTimeOffset.UtcNow;

            foreach (var entry in changeTracker.Entries<BaseEntity>()
               .Where(e => e.State == EntityState.Added
                        || e.State == EntityState.Modified))
            {
                if (entry.State != EntityState.Added)
                {
                    entry.Entity.UpdatedDateUtc ??= now;
                    entry.Entity.UpdatedBy ??= userId;
                }
                else
                {
                    entry.Entity.CreatedBy = entry.Entity.CreatedBy != 0 ? entry.Entity.CreatedBy : userId;
                    entry.Entity.CreatedDateUtc = entry.Entity.CreatedDateUtc == DateTimeOffset.MinValue ? now : entry.Entity.CreatedDateUtc;
                }
            }
        }

        public static void RevertChanges(this ChangeTracker changeTracker)
        {
            foreach (var entry in changeTracker.Entries().Where(e => e.Entity != null).ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;  //Revert changes made to deleted entity.
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }

        public static DataTable GetDataTable(this DbContext context,
            string sqlQuery,
            List<SqlParameter> parameters = null,
            CommandType commandType = CommandType.Text)
        {
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(context.Database.GetDbConnection());

            using var cmd = dbFactory.CreateCommand();
            cmd.Connection = context.Database.GetDbConnection();
            cmd.CommandType = commandType;
            cmd.CommandText = sqlQuery;
            if (parameters != null && parameters.Any())
            {
                cmd.Parameters.AddRange(parameters.ToArray());
            }

            using DbDataAdapter adapter = dbFactory.CreateDataAdapter();
            adapter.SelectCommand = cmd;
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            return dt;
        }

        public static DataSet GetDataSet(this DbContext context, string sqlQuery, List<SqlParameter> parameters = null)
        {
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(context.Database.GetDbConnection());

            using var cmd = dbFactory.CreateCommand();
            cmd.Connection = context.Database.GetDbConnection();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sqlQuery;
            if (parameters != null && parameters.Any())
            {
                cmd.Parameters.AddRange(parameters.ToArray());
            }
            using DbDataAdapter adapter = dbFactory.CreateDataAdapter();
            adapter.SelectCommand = cmd;

            DataSet ds = new DataSet();
            adapter.Fill(ds);

            return ds;
        }
    }
}
