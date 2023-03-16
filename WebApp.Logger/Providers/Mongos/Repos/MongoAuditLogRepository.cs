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
using WebApp.Logger.Providers.Mongos;
using WebApp.Logger.Providers.Mongos.Repos.Documents;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Providers.Mongos.Repos
{
    public class MongoAuditLogRepository : IAuditLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<MongoAuditLogRepository> _logger;
        private readonly IMongoRepository<AuditLogDocument> _auditRepository;
        private readonly LogOption _logOption;
        public MongoAuditLogRepository(DapperContext dapper,
            ILogger<MongoAuditLogRepository> logger,
            IMongoRepository<AuditLogDocument> auditRepository,
            IOptions<LogOption> logOption)
        {
            _dapper = dapper;
            _logger = logger;
            _auditRepository = auditRepository;
            _logOption = logOption.Value;
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            return await _auditRepository.GetPageAsync(pager);
        }

        public async Task AddAsync(AuditEntry auditEntry)
        {
            var auditDocument = auditEntry.ToAuditModel().ToDocument();

            await _auditRepository.InsertAsync(auditDocument);
        }

        public async Task AddAsync(List<AuditEntry> auditEntries)
        {
            var auditModel = auditEntries.ToAuditModel(_logOption);
            auditModel = auditModel.PrepareAuditModel(_logOption);
            var auditDocument = auditModel.Select(e => e.ToDocument());

            await _auditRepository.InsertManyAsync(auditDocument);
        }

        public async Task RetentionAsync(DateTime dateTime)
        {
            await _auditRepository.DeleteManyAsync(x => x.DateTime <= dateTime);
        }
    }
}
