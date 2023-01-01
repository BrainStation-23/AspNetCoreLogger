using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Logger.Loggers.Providers.Mongos;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Mongos;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Loggers.Repositories
{
    public class MongoExceptionLogRepository : IExceptionLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<MongoExceptionLogRepository> _logger;
        private readonly IMongoRepository<ErrorLogDocument> _errorRepository;
        private readonly LogOption _logOptions;

        public MongoExceptionLogRepository(DapperContext dapper,
            ILogger<MongoExceptionLogRepository> logger,
            IMongoRepository<ErrorLogDocument> errorRepository,
            IOptions<LogOption>logOption)
        {
            _dapper = dapper;
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

            errorModel = errorModel.PrepareErrorModel(_logOptions);

            //if (errorModel.Url.Contains("/Log/"))
            //    return;
            errorModel = errorModel.DeserializeErrorModel();
            var errorDocument = errorModel.ToDocument();

            await _errorRepository.InsertAsync(errorDocument);
        }

        public async Task AddAsync(List<ErrorModel> errorModels)
        {
            var errorDocuments = errorModels.Where(e => !e.Url.Contains("/Log")).Select(e => e.ToDocument());

            await _errorRepository.InsertManyAsync(errorDocuments);
        }
    }
}
