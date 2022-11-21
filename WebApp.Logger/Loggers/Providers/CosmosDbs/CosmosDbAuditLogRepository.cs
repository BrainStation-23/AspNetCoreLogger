using Microsoft.Extensions.Logging;
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

        public CosmosDbAuditLogRepository(DapperContext dapper,
            ILogger<RouteLogRepository> logger,
            ICosmosDbRepository<AuditLogItem> auditRepository)
        {
            _dapper = dapper;
            _logger = logger;
            _auditRepository = auditRepository;
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            var response = await _auditRepository.GetsAsync("", pager);

            return response;
        }

        public async Task AddAsync(List<AuditEntry> auditEntries)
        {
            var auditModels = auditEntries.ToAuditModel(false);
            var auditItems = auditModels.Select(e => e.ToItem()).ToList();

            await _auditRepository.InsertManyAsync(auditItems);
        }

        public async Task AddAsync(AuditEntry auditEntry)
        {
            var model = auditEntry.ToAuditModel(false).ToItem();
            await _auditRepository.InsertAsync(model);
        }
    }
}
