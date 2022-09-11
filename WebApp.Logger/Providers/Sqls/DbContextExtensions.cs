using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using WebApp.Common.Exceptions;
using WebApp.Common.Sqls;
using WebApp.Logger.Enums;
using WebApp.Logger.Models;

namespace WebApp.Common.Contexts
{
    public static class DbContextExtensions
    {
        private static bool HasChanges(PropertyValues originalEntry, EntityEntry currentValues)
        {
            bool isChanges = false;
            var ignorePropertyName = typeof(BaseEntity).GetProperties().Select(e => e.Name).ToList();

            foreach (var property in currentValues.Properties)
            {
                string propertyName = property.Metadata.Name;

                switch (currentValues.State)
                {
                    case EntityState.Added:
                        if (originalEntry is null && property.CurrentValue is not null)
                        {
                            isChanges = true;
                            break;
                        }
                        break;
                    case EntityState.Deleted:
                        if (originalEntry is not null)
                        {
                            isChanges = true;
                            break;
                        }
                        break;
                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            if (ignorePropertyName.Contains(propertyName))
                                continue;

                            var currentValue = property.CurrentValue?.ToString();
                            var originalValue = originalEntry[propertyName]?.ToString();
                            if (currentValue != originalValue)
                            {
                                isChanges = true;
                                break;
                            }
                        }
                        break;
                }
            }

            return isChanges;
        }

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
                )
                    continue;

                if (!string.IsNullOrEmpty(ignoreEntity))
                    if (entry.Entity.GetType().Name == ignoreEntity)
                        continue;

                var originalEntry = entry.GetDatabaseValues();
                var hasChanges = HasChanges(originalEntry, entry);
                if (!hasChanges) continue;

                var auditEntry = new AuditEntry(entry)
                {
                    TableName = entry.Entity.GetType().Name,
                    UserId = userId
                };
                auditEntries.Add(auditEntry);

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
                                var tableName = entry.Metadata.ClrType.Name;

                                if (originalEntry == null) throw new AppException($"{tableName} Entry data not found");

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
            var ignorePropertyName = typeof(BaseEntity).GetProperties().Select(e => e.Name).ToList();


            foreach (var entry in changeTracker.Entries<BaseEntity>().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (ignorePropertyName.Contains(propertyName))
                        entry.Property(propertyName).IsModified = false;

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

        public static object GetDatabaseInfo(this IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("WebAppConnection");
            var builder = new SqlConnectionStringBuilder(connectionString);

            return new
            {
                server = builder.DataSource,
                catalog = builder.InitialCatalog
            };
        }

    }
}
