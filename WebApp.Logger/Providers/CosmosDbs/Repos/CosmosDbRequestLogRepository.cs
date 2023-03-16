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

namespace WebApp.Logger.Providers.CosmosDbs.Repos
{
    public class CosmosDbRequestLogRepository : IRequestLogRepository
    {
        private readonly ILogger<CosmosDbRequestLogRepository> _logger;
        private readonly ICosmosDbRepository<RequestLogItem> _routeRepository;
        private readonly LogOption _logOption;

        public CosmosDbRequestLogRepository(ILogger<CosmosDbRequestLogRepository> logger,
            ICosmosDbRepository<RequestLogItem> routeRepository,
            IOptions<LogOption> logOption)
        {
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

        public async Task RetentionAsync(DateTime dateTime)
        {
            string date = dateTime.ToString("yyyy-MM-dd");//'T'HH: mm:ss.SSS'Z'
            await _routeRepository.DeleteAsync(date, _logOption.Log.Request.GetType().Name.ToString().ToLower());
        }
    }
}
