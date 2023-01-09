using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private readonly LogOption _logOption;
        public CosmosDbSqlLogRepository(DapperContext dapper,
            ILogger<CosmosDbSqlLogRepository> logger,
            ICosmosDbRepository<SqlLogItem> sqlRepository,
            IOptions<LogOption> logOption)
        {
            _dapper = dapper;
            _logger = logger;
            _sqlRepository = sqlRepository;
            _logOption = logOption.Value;
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
            var model = sqlModel.PrepareSqlModel(_logOption).ToItem();
            await _sqlRepository.InsertAsync(model);
        }
    }
}
