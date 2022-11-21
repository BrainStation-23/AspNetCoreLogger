using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Logger.Loggers.Providers.CosmosDbs.Items;
using WebApp.Logger.Loggers.Providers.Mongos;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Mongos;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Loggers.Repositories
{
    public class MongoSqlLogRepository : ISqlLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<MongoSqlLogRepository> _logger;
        private readonly IMongoRepository<SqlLogDocument> _sqlRepository;

        public MongoSqlLogRepository(DapperContext dapper,
            ILogger<MongoSqlLogRepository> logger,
            IMongoRepository<SqlLogDocument> sqlRepository)
        {
            _dapper = dapper;
            _logger = logger;
            _sqlRepository = sqlRepository;
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            //Expression<Func<RequestLogDocument, bool>> isDate = s => s.DateTime != null;
            //var filter = Builders<RequestLogDocument>.Filter.Where(x => x.DateTime != null);
            return await _sqlRepository.GetPageAsync(pager);
        }

        public async Task AddAsync(SqlModel sqlModel)
        {
            if (sqlModel.Url.Contains("/Log/"))
                return;

            var sqlDocument = sqlModel.ToDocument();

            await _sqlRepository.InsertAsync(sqlDocument);
        }

        public async Task AddAsync(List<SqlModel> sqlModels)
        {
            var sqlDocuments = sqlModels.Where(e => !e.Url.Contains("/Log")).Select(e => e.ToDocument());

            await _sqlRepository.InsertManyAsync(sqlDocuments);
        }
    }
}
