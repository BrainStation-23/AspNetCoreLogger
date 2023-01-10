using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Logger.Loggers.Providers.CosmosDbs;
using WebApp.Logger.Loggers.Providers.CosmosDbs.Items;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Mongos;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Loggers.Repositories
{
    public class CosmosDbAuditLogRepository : IAuditLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<RouteLogRepository> _logger;
        private readonly ICosmosDbRepository<AuditLogItem> _auditRepository;
        private readonly LogOption _logOption;

        public CosmosDbAuditLogRepository(DapperContext dapper,
            ILogger<RouteLogRepository> logger,
            ICosmosDbRepository<AuditLogItem> auditRepository,
            IOptions<LogOption> logOptions)
        {
            _dapper = dapper;
            _logger = logger;
            _auditRepository = auditRepository;
            _logOption = logOptions.Value;
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            var response = await _auditRepository.GetsAsync("", pager);

            return response;
        }

        public async Task AddAsync(List<AuditEntry> auditEntries)
        {
            var auditModels = auditEntries.ToAuditModel(_logOption).PrepareAuditModel(_logOption);
            var auditItems = auditModels.Select(e => e.ToItem()).ToList();

            await _auditRepository.InsertManyAsync(auditItems);
        }

        public async Task AddAsync(AuditEntry auditEntry)
        {
            var model = auditEntry.ToAuditModel(false).PrepareAuditModel(_logOption).ToItem();
            await _auditRepository.InsertAsync(model);
        }
        public async Task RetentionAsync(DateTime dateTime)
        {
            //todo
        }
    }
}
