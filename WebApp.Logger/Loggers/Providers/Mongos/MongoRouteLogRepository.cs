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
    public class MongoRouteLogRepository : IRouteLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<MongoRouteLogRepository> _logger;
        private readonly IMongoRepository<RequestLogDocument> _RequestRepository;
        private readonly LogOption _logOption;
        public MongoRouteLogRepository(DapperContext dapper,
            ILogger<MongoRouteLogRepository> logger,
            IMongoRepository<RequestLogDocument> RequestRepository,
            IOptions<LogOption>logOption)
        {
            _dapper = dapper;
            _logger = logger;
            _RequestRepository = RequestRepository;
            _logOption=logOption.Value;
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            //Expression<Func<RequestLogDocument, bool>> isDate = s => s.DateTime != null;
            //var filter = Builders<RequestLogDocument>.Filter.Where(x => x.DateTime != null);
            return await _RequestRepository.GetPageAsync(pager);
        }

        public async Task AddAsync(RequestModel requestModel)
        {
            if (LogOptionExtension.SkipRequestLog(requestModel, _logOption))
                return;

            requestModel = requestModel.PrepareRequestModel(_logOption);
            //if (requestModel.Url.Contains("/Log/"))
            //    return;
            requestModel = requestModel.DeserializeRequestModel();
            var requestDocument = requestModel.ToDocument();

            await _RequestRepository.InsertAsync(requestDocument);
        }

        public async Task AddAsync(List<RequestModel> requestModels)
        {
            var requestDocuments = requestModels.Where(e => !e.Url.Contains("/Log")).Select(e => e.ToDocument());

            await _RequestRepository.InsertManyAsync(requestDocuments);
        }
    }
}
