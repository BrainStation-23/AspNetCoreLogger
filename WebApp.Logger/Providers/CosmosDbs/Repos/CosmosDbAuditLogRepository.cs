using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Logger.Contracts;
using WebApp.Logger.Loggers;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.CosmosDbs.Repos.Items;
using WebApp.Logger.Providers.Mongos;
using WebApp.Logger.Providers.Sqls;
using WebApp.Logger.Providers.Sqls.Repos;

namespace WebApp.Logger.Providers.CosmosDbs.Repos
{
    public class CosmosDbAuditLogRepository : IAuditLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<RequestLogRepository> _logger;
        private readonly ICosmosDbRepository<AuditLogItem> _auditRepository;
        private readonly LogOption _logOption;

        public CosmosDbAuditLogRepository(DapperContext dapper,
            ILogger<RequestLogRepository> logger,
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
            string date = dateTime.ToString("yyyy-MM-dd");//'T'HH: mm:ss.SSS'Z'
            await _auditRepository.DeleteAsync(date, _logOption.Log.Audit.GetType().Name.ToString().ToLower());
        }
    }
}
