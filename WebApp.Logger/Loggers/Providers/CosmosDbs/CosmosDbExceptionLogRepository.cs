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
    public class CosmosDbExceptionLogRepository : IExceptionLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<RouteLogRepository> _logger;
        private readonly ICosmosDbRepository<ErrorLogItem> _errorRepository;

        public CosmosDbExceptionLogRepository(DapperContext dapper,
            ILogger<RouteLogRepository> logger,
            ICosmosDbRepository<ErrorLogItem> errorRepository)
        {
            _dapper = dapper;
            _logger = logger;
            _errorRepository = errorRepository;
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            var response = await _errorRepository.GetsAsync("", pager);

            return response;
        }

        public async Task AddAsync(List<ErrorModel> errorModels)
        {
            //var errorItems = errorModels.Select(e => e.ToItem()).ToList();

            //await _errorRepository.InsertManyAsync(errorItems);
        }

        public async Task AddAsync(ErrorModel errorModel)
        {
            //var model = errorModel.ToItem();
            //await _errorRepository.InsertAsync(model);
        }
    }
}
