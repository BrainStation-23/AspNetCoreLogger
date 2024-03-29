﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApp.Logger.Enums;
using WebApp.Logger.Extensions;
using WebApp.Logger.Loggers;

namespace WebApp.Logger.Models
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        public EntityEntry Entry { get; }

        public string TraceId { get; set; }
        public string RequestId { get; set; }
        public long UserId { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string ApplicationName { get; set; }
        public string SchemaName { get; set; }
        public string TableName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public AuditType AuditType { get; set; }
        public List<string> ChangedColumnNames { get; } = new List<string>();
        public IDictionary<string, object> Changes { get; set; } = new Dictionary<string, object>();
    }

    public static class AuditModelMapper
    {
        public static AuditModel ToAuditModel(this AuditEntry auditEntry, bool objectValue = true)
        {
            //if (objectValue)
            //    return new AuditModel
            //    {
            //        UserId = auditEntry.UserId,
            //        Type = auditEntry.AuditType.ToString(),
            //        SchemaName = auditEntry.SchemaName,
            //        TableName = auditEntry.TableName,
            //        DateTime = DateTime.Now,
            //        PrimaryKey = auditEntry.KeyValues,
            //        OldValues = auditEntry.OldValues,
            //        NewValues = auditEntry.NewValues,
            //        AffectedColumns = auditEntry.ChangedColumnNames,
            //        CreatedBy = 0,
            //        CreatedDateUtc = DateTime.UtcNow,
            //        TraceId = auditEntry.TraceId
            //    };

            //return new AuditModel
            //{
            //    UserId = auditEntry.UserId,
            //    Type = auditEntry.AuditType.ToString(),
            //    TableName = auditEntry.TableName,
            //    SchemaName = auditEntry.SchemaName,
            //    DateTime = DateTime.Now,
            //    PrimaryKey = JsonConvert.SerializeObject(auditEntry.KeyValues),
            //    OldValues = auditEntry.OldValues.Count == 0 ? null : JsonConvert.SerializeObject(auditEntry.OldValues),
            //    NewValues = auditEntry.NewValues.Count == 0 ? null : JsonConvert.SerializeObject(auditEntry.NewValues),
            //    AffectedColumns = auditEntry.ChangedColumnNames.Count == 0 ? null : JsonConvert.SerializeObject(auditEntry.ChangedColumnNames),
            //    CreatedBy = 0,
            //    CreatedDateUtc = DateTime.UtcNow,
            //    TraceId = auditEntry.TraceId
            //};
            //if (objectValue)
            //    return new AuditModel
            //    {
            //        UserId = auditEntry.UserId,
            //        Type = auditEntry.AuditType.ToString(),
            //        SchemaName = auditEntry.SchemaName,
            //        TableName = auditEntry.TableName,
            //        DateTime = DateTime.Now,
            //        PrimaryKey = auditEntry.KeyValues,
            //        OldValues = auditEntry.OldValues,
            //        NewValues = auditEntry.NewValues,
            //        AffectedColumns = auditEntry.ChangedColumnNames,
            //        CreatedBy = 0,
            //        CreatedDateUtc = DateTime.UtcNow,
            //        TraceId = auditEntry.TraceId
            //    };
            //else
            //    return new AuditModel
            //    {
            //        UserId = auditEntry.UserId,
            //        Type = auditEntry.AuditType.ToString(),
            //        SchemaName = auditEntry.SchemaName,
            //        TableName = auditEntry.TableName,
            //        DateTime = DateTime.Now,
            //        PrimaryKey = auditEntry.KeyValues.Count == 0 ? null : JsonConvert.SerializeObject(auditEntry.KeyValues),
            //        OldValues = auditEntry.OldValues.Count == 0 ? null : JsonConvert.SerializeObject(auditEntry.OldValues),
            //        NewValues = auditEntry.NewValues.Count == 0 ? null : JsonConvert.SerializeObject(auditEntry.NewValues),
            //        AffectedColumns = auditEntry.ChangedColumnNames.Count == 0 ? null : JsonConvert.SerializeObject(auditEntry.ChangedColumnNames),
            //        CreatedBy = 0,
            //        CreatedDateUtc = DateTime.UtcNow,
            //        TraceId = auditEntry.TraceId
            //    };

            return new AuditModel
            {
                UserId = auditEntry.UserId,
                Type = auditEntry.AuditType.ToString(),
                SchemaName = auditEntry.SchemaName,
                TableName = auditEntry.TableName,
                DateTime = DateTime.Now,
                PrimaryKey = auditEntry.KeyValues,
                OldValues = auditEntry.OldValues,
                NewValues = auditEntry.NewValues,
                AffectedColumns = auditEntry.ChangedColumnNames,
                CreatedBy = 0,
                CreatedDateUtc = DateTime.UtcNow,
                TraceId = auditEntry.TraceId,
                ApplicationName = auditEntry.ApplicationName,
                ControllerName = auditEntry.ControllerName,
                ActionName = auditEntry.ActionName
            };
        }

        public static List<AuditModel> ToAuditModel(this List<AuditEntry> auditEntry, LogOption logOption)
        {
            var objectValue = false;
            var providerType = logOption.ProviderType.ToEnum(ProviderType.MSSql);

            switch (providerType)
            {
                case ProviderType.MSSql:
                    objectValue = false;
                    break;
                case ProviderType.File:
                case ProviderType.MongoDb:
                case ProviderType.CosmosDb:
                    objectValue = true;
                    break;
                default:
                    break;
            }

            return auditEntry.Select(e => e.ToAuditModel(objectValue)).ToList();
        }
    }
}
