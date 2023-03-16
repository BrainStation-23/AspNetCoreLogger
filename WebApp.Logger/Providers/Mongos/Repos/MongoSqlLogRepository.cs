using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Logger.Contracts;
using WebApp.Logger.Loggers;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Mongos.Repos.Documents;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Providers.Mongos.Repos
{
    public class MongoSqlLogRepository : ISqlLogRepository
    {
        private readonly ILogger<MongoSqlLogRepository> _logger;
        private readonly IMongoRepository<SqlLogDocument> _sqlRepository;
        private readonly LogOption _logOptions;

        public MongoSqlLogRepository(ILogger<MongoSqlLogRepository> logger,
            IMongoRepository<SqlLogDocument> sqlRepository,
            IOptions<LogOption> logOptions)
        {
            _logger = logger;
            _sqlRepository = sqlRepository;
            _logOptions = logOptions.Value;
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            //Expression<Func<RequestLogDocument, bool>> isDate = s => s.DateTime != null;
            //var filter = Builders<RequestLogDocument>.Filter.Where(x => x.DateTime != null);
            return await _sqlRepository.GetPageAsync(pager);
        }

        public async Task AddAsync(SqlModel sqlModel)
        {
            //if (sqlModel.Url.Contains("/Log/"))
            //    return;
            if (LogOptionExtension.SkipSqlLog(sqlModel, _logOptions))
                return;

            sqlModel = sqlModel.PrepareSqlModel(_logOptions);

            var sqlDocument = sqlModel.ToDocument();

            await _sqlRepository.InsertAsync(sqlDocument);
        }

        public async Task AddAsync(List<SqlModel> sqlModels)
        {
            var sqlDocuments = sqlModels.Where(e => !e.Url.Contains("/Log")).Select(e => e.PrepareSqlModel(_logOptions).ToDocument());

            await _sqlRepository.InsertManyAsync(sqlDocuments);
        }

        public async Task RetentionAsync(DateTime dateTime)
        {
            await _sqlRepository.DeleteManyAsync(x => x.CreatedDateUtc <= dateTime);
        }
    }
}
