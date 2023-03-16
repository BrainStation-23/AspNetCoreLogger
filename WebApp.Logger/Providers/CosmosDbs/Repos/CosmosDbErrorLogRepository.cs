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
    public class CosmosDbErrorLogRepository : IErrorLogRepository
    {
        private readonly ILogger<CosmosDbErrorLogRepository> _logger;
        private readonly ICosmosDbRepository<ErrorLogItem> _errorRepository;
        private readonly LogOption _logOption;

        public CosmosDbErrorLogRepository(ILogger<CosmosDbErrorLogRepository> logger,
            ICosmosDbRepository<ErrorLogItem> errorRepository,
            IOptions<LogOption> logOption)
        {
            _logger = logger;
            _errorRepository = errorRepository;
            _logOption = logOption.Value;
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
            await Task.CompletedTask;
        }

        public async Task AddAsync(ErrorModel errorModel)
        {
            var model = errorModel.PrepareErrorModel(_logOption).ToItem();
            await _errorRepository.InsertAsync(model);
        }
        public async Task RetentionAsync(DateTime dateTime)
        {
            string date = dateTime.ToString("yyyy-MM-dd");//'T'HH: mm:ss.SSS'Z'
            await _errorRepository.DeleteAsync(date, _logOption.Log.Error.GetType().Name.ToString().ToLower());
        }
    }
}
