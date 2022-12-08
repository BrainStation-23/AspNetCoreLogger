﻿using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Logger.Loggers.Providers.Mongos;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Mongos;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Loggers.Repositories
{
    public class MongoAuditLogRepository : IAuditLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<MongoAuditLogRepository> _logger;
        private readonly IMongoRepository<AuditLogDocument> _auditRepository;

        public MongoAuditLogRepository(DapperContext dapper,
            ILogger<MongoAuditLogRepository> logger,
            IMongoRepository<AuditLogDocument> auditRepository)
        {
            _dapper = dapper;
            _logger = logger;
            _auditRepository = auditRepository;
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
            var auditModel = auditEntries.ToAuditModel();
            var auditDocument = auditModel.Select(e => e.ToDocument());

            await _auditRepository.InsertManyAsync(auditDocument);
        }
    }
}