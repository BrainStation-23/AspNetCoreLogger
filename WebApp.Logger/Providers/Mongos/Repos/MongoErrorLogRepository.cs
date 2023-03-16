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
    public class MongoErrorLogRepository : IErrorLogRepository
    {
        private readonly ILogger<MongoErrorLogRepository> _logger;
        private readonly IMongoRepository<ErrorLogDocument> _errorRepository;
        private readonly LogOption _logOptions;

        public MongoErrorLogRepository(ILogger<MongoErrorLogRepository> logger,
            IMongoRepository<ErrorLogDocument> errorRepository,
            IOptions<LogOption> logOption)
        {
            _logger = logger;
            _errorRepository = errorRepository;
            _logOptions = logOption.Value;
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            return await _errorRepository.GetPageAsync(pager);
        }

        public async Task AddAsync(ErrorModel errorModel)
        {
            if (LogOptionExtension.SkipErrorLog(errorModel, _logOptions))
                return;

            errorModel = errorModel.DeserializeErrorModel().PrepareErrorModel(_logOptions);

            //if (errorModel.Url.Contains("/Log/"))
            //    return;
            var errorDocument = errorModel.ToDocument();

            await _errorRepository.InsertAsync(errorDocument);
        }

        public async Task AddAsync(List<ErrorModel> errorModels)
        {
            var errorDocuments = errorModels.Where(e => !e.Url.Contains("/Log")).Select(e => e.DeserializeErrorModel().PrepareErrorModel(_logOptions).ToDocument());

            await _errorRepository.InsertManyAsync(errorDocuments);
        }

        public async Task RetentionAsync(DateTime dateTime)
        {
            await _errorRepository.DeleteManyAsync(x => x.CreatedDateUtc <= dateTime);
        }
    }
}
