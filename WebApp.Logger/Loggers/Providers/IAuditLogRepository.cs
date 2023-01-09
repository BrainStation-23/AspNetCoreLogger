using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Loggers.Repositories
{
    public interface IAuditLogRepository
    {
        Task AddAsync(AuditEntry auditEntry);
        Task AddAsync(List<AuditEntry> auditEntries);
        Task<dynamic> GetPageAsync(DapperPager pager);
        Task RetentionAsync(DateTime dateTime);
    }
}