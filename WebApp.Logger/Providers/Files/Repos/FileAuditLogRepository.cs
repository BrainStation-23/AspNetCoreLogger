using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Logger.Contracts;
using WebApp.Logger.Extensions;
using WebApp.Logger.Loggers;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Providers.Files.Repos
{
    public class FileAuditLogRepository : IAuditLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<FileAuditLogRepository> _logger;
        private readonly LogOption _logOptions;

        public FileAuditLogRepository(DapperContext dapper,
            ILogger<FileAuditLogRepository> logger,
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
                _logger.LogError(nameof(FileAuditLogRepository), exception);
            }

            await Task.CompletedTask;
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
                _logger.LogError(nameof(FileAuditLogRepository), exception);
            }

            await Task.FromResult(Task.CompletedTask);
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            var fileConfig = _logOptions.Provider.File;
            var auditLogs = fileConfig.Path.GetFilenames(LogType.Audit.ToString());

            return await Task.FromResult(auditLogs);
        }

        public async Task RetentionAsync(DateTime dateTime)
        {
            dateTime.RetentionFileLogs(_logOptions.Provider.File.Path, LogType.Audit.ToString());

            await Task.FromResult(Task.CompletedTask);
        }
    }
}
