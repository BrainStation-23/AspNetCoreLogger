using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Logger.Extensions;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Loggers.Repositories
{
    public class AuditFileLogRepository : IAuditLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<RouteLogRepository> _logger;
        private readonly LogOption _logOptions;

        public AuditFileLogRepository(DapperContext dapper,
            ILogger<RouteLogRepository> logger,
            IOptions<LogOption> logOption)
        {
            _dapper = dapper;
            _logger = logger;
            _logOptions = logOption.Value;
        }

        public async Task AddAsync(AuditEntry auditEntry)
        {
            var fileConfig = _logOptions.Provider.File;

            try
            {
                var auditModel = auditEntry.ToAuditModel();
                auditModel = auditModel.PrepareAuditModel(_logOptions);
                FileExtension.LogWrite(fileConfig, auditModel);
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(AuditFileLogRepository), exception);
            }
        }

        public async Task AddAsync(List<AuditEntry> auditEntries)
        {
            var fileConfig = _logOptions.Provider.File;
            try
            {
                var auditModels = auditEntries.ToAuditModel(_logOptions);
                auditModels = auditModels.PrepareAuditModel(_logOptions);
                FileExtension.LogWrite(fileConfig, auditModels);
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(AuditFileLogRepository), exception);
            }
        }
        
        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            dynamic routeLogs = null;

            return routeLogs;
        }

        public async Task RetentionAsync(DateTime dateTime)
        {

        }
    }
}
