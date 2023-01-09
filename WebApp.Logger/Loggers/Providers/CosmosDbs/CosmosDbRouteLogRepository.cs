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
    public class CosmosDbRouteLogRepository : IRouteLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<RouteLogRepository> _logger;
        private readonly ICosmosDbRepository<RequestLogItem> _routeRepository;
        private readonly LogOption _logOption;

        public CosmosDbRouteLogRepository(DapperContext dapper,
            ILogger<RouteLogRepository> logger,
            ICosmosDbRepository<RequestLogItem> routeRepository,
            IOptions<LogOption> logOption)
        {
            _dapper = dapper;
            _logger = logger;
            _routeRepository = routeRepository;
            _logOption = logOption.Value;
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            var response = await _routeRepository.GetsAsync("", pager);

            return response;
        }

        public async Task AddAsync(List<RequestModel> requestModels)
        {
            var routeItems = requestModels.Select(e => e.ToItem()).ToList();

            await _routeRepository.InsertManyAsync(routeItems);
        }

        public async Task AddAsync(RequestModel requestModel)
        {
            var model = requestModel.PrepareRequestModel(_logOption).ToItem();
            await _routeRepository.InsertAsync(model);
        }
    }
}
