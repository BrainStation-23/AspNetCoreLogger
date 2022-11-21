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
    public class CosmosDbSqlLogRepository : ISqlLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<CosmosDbSqlLogRepository> _logger;
        private readonly ICosmosDbRepository<SqlLogItem> _sqlRepository;

        public CosmosDbSqlLogRepository(DapperContext dapper,
            ILogger<CosmosDbSqlLogRepository> logger,
            ICosmosDbRepository<SqlLogItem> sqlRepository)
        {
            _dapper = dapper;
            _logger = logger;
            _sqlRepository = sqlRepository;
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            var response = await _sqlRepository.GetsAsync("", pager);

            return response;
        }

        public async Task AddAsync(List<SqlModel> sqlModels)
        {
            //var sqlItems = sqlModels.Select(e => e.ToItem()).ToList();
            //await _sqlRepository.InsertManyAsync(sqlItems);
        }

        public async Task AddAsync(SqlModel sqlModel)
        {
            //var model = sqlModel.ToItem();
            //await _sqlRepository.InsertAsync(model);
        }
    }
}
