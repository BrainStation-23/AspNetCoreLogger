﻿using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Common.Serialize;
using WebApp.Logger.Extensions;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Loggers.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<RouteLogRepository> _logger;
        private readonly LogOption _logOption;
        public AuditLogRepository(DapperContext dapper,
            ILogger<RouteLogRepository> logger,
            IOptions<LogOption> logOptions)
        {
            _dapper = dapper;
            _logger = logger;
            _logOption = logOptions.Value;
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            dynamic auditLogs;
            var query = @"SELECT * FROM [dbo].[AuditTrailLogs]
                            ORDER BY [Id] DESC
                            OFFSET @Offset ROWS 
                            FETCH NEXT  @Next   ROWS ONLY";

            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    var auditLogsEntities = await connection.QueryAsync(query, pager);
                    var auditLogUnescapeString = JsonSerializeExtentions.ToJson(auditLogsEntities).JsonUnescaping();
                    auditLogs = JArray.Parse(auditLogUnescapeString);
                }

                return auditLogs;
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(RouteLogRepository), exception);
                throw;
            }
        }

        public async Task AddAsync(AuditEntry auditEntry)
        {
            var createdDateUtc = DateTime.UtcNow.ToString();
            var query = @"INSERT INTO [dbo].[AuditTrailLogs]
                               ([UserId]
                               ,[Type]
                               ,[TableName]
                               ,[DateTime]
                               ,[OldValues]
                               ,[NewValues]
                               ,[AffectedColumns]
                               ,[PrimaryKey]
                               ,[CreatedBy]
                               ,[CreatedDateUtc]
                               ,[UpdatedBy]
                               ,[UpdatedDateUtc]
                               ,[TraceId]
                               ,[ControllerName]
                               ,[ActionName]
                               ,[ApplicationName])
                         VALUES
                               ( @UserId
                               , @Type
                               , @TableName
                               , @DateTime
                               , @OldValues
                               , @NewValues
                               , @AffectedColumns
                                ,@PrimaryKey
                               , @CreatedBy
                               , @CreatedDateUtc
                               , @UpdatedBy
                               , @UpdatedDateUtc
                               , @TraceId
                               , @ControllerName
                               , @ActionName
                               , @ApplicationName)";

            try
            {
                using var connection = _dapper.CreateConnection();
                await connection.ExecuteAsync(query, new
                {
                    UserId = auditEntry.UserId,
                    Type = auditEntry.AuditType.ToString(),
                    TraceId = auditEntry.TraceId,
                    ControllerName = auditEntry.ControllerName,
                    ActionName = auditEntry.ActionName,
                    ApplicationName = auditEntry.ApplicationName,
                    TableName = auditEntry.TableName,
                    DateTime = DateTime.Now,
                    PrimaryKey = JsonConvert.SerializeObject(auditEntry.KeyValues),
                    OldValues = auditEntry.OldValues.Count == 0 ? null : JsonConvert.SerializeObject(auditEntry.OldValues),
                    NewValues = auditEntry.NewValues.Count == 0 ? null : JsonConvert.SerializeObject(auditEntry.NewValues),
                    AffectedColumns = auditEntry.ChangedColumnNames.Count == 0 ? null : JsonConvert.SerializeObject(auditEntry.ChangedColumnNames),
                    CreatedBy = 0,
                    CreatedDateUtc = createdDateUtc,
                    UpdatedBy = 0,
                    UpdatedDateUtc = createdDateUtc
                });
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(RouteLogRepository), exception);
            }
        }

        public async Task AddAsync(List<AuditEntry> auditEntries)
        {
            var query = @"INSERT INTO [dbo].[AuditTrailLogs]
                               ([UserId]
                               ,[Type]
                               ,[TableName]
                               ,[DateTime]
                               ,[OldValues]
                               ,[NewValues]
                               ,[AffectedColumns]
                               ,[PrimaryKey]
                               ,[CreatedBy]
                               ,[CreatedDateUtc]
                               ,[UpdatedBy]
                               ,[UpdatedDateUtc]
                               ,[TraceId]
                               ,[ControllerName]
                               ,[ActionName]
                               ,[ApplicationName])
                         VALUES
                               ( @UserId
                               , @Type
                               , @TableName
                               , @DateTime
                               , @OldValues
                               , @NewValues
                               , @AffectedColumns
                                ,@PrimaryKey
                               , @CreatedBy
                               , @CreatedDateUtc
                               , @UpdatedBy
                               , @UpdatedDateUtc
                               , @TraceId
                               , @ControllerName
                               , @ActionName
                               , @ApplicationName)";

            try
            {
                using var connection = _dapper.CreateConnection();
                var models = auditEntries.ToAuditModel(_logOption);
                models = models.PrepareAuditModel(_logOption);
                models = models.SerializeAuditModel();

                await connection.ExecuteAsync(query, models);
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(RouteLogRepository), exception);
            }
        }

        public async Task RetentionAsync(DateTime dateTime)
        {
            string date = dateTime.ToString();//"2023-01-04 06:11:12.2333333"
            var query = $"delete from [dbo].[AuditTrailLogs] where DateTime <= '{date}'";
            using var connection = _dapper.CreateConnection();
            await connection.ExecuteAsync(query);
        }
    }
}
