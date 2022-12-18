using Dapper;
using MassTransit.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Common.Serialize;
using WebApp.Logger.Extensions;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Loggers.Repositories
{
    public class AuditFileLogRepository : IAuditLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<RouteLogRepository> _logger;
        private readonly LogOption _logOption;

        public AuditFileLogRepository(DapperContext dapper,
            ILogger<RouteLogRepository> logger,
            IOptions<LogOption> logOption)
        {
            _dapper = dapper;
            _logger = logger;
            _logOption = logOption.Value;
        }

        public async Task AddAsync(AuditEntry auditEntry)
        {
            var fileConfig = _logOption.Provider.File;
            var auditModel = auditEntry.ToAuditModel();
            try
            {
                FileExtension.LogWrite(fileConfig.Path, null, auditModel);
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(AuditFileLogRepository), exception);
            }
        }
        

        public async Task AddAsync(List<AuditEntry> auditEntries)
        {
            var fileConfig = _logOption.Provider.File;
            try
            {
                var auditModels = auditEntries.ToAuditModel(false);
                FileExtension.LogWrite(fileConfig.Path, null, auditModels);
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(AuditFileLogRepository), exception);
            }
        }
        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            dynamic auditLogs;
            var query = @"SELECT * FROM [dbo].[AuditLogs]
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
    }
}
