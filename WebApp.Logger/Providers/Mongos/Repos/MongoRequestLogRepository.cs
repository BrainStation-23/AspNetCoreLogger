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
    public class MongoRequestLogRepository : IRequestLogRepository
    {
        private readonly ILogger<MongoRequestLogRepository> _logger;
        private readonly IMongoRepository<RequestLogDocument> _requestRepository;
        private readonly LogOption _logOption;

        public MongoRequestLogRepository(ILogger<MongoRequestLogRepository> logger,
            IMongoRepository<RequestLogDocument> RequestRepository,
            IOptions<LogOption> logOption)
        {
            _logger = logger;
            _requestRepository = RequestRepository;
            _logOption = logOption.Value;
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            //Expression<Func<RequestLogDocument, bool>> isDate = s => s.DateTime != null;
            //var filter = Builders<RequestLogDocument>.Filter.Where(x => x.DateTime != null);
            return await _requestRepository.GetPageAsync(pager);
        }

        public async Task AddAsync(RequestModel requestModel)
        {
            if (LogOptionExtension.SkipRequestLog(requestModel, _logOption))
                return;

            requestModel = requestModel.DeserializeRequestModel().PrepareRequestModel(_logOption);
            //if (requestModel.Url.Contains("/Log/"))
            //    return;
            var requestDocument = requestModel.ToDocument();

            await _requestRepository.InsertAsync(requestDocument);
        }

        public async Task AddAsync(List<RequestModel> requestModels)
        {
            var requestDocuments = requestModels.Where(e => !e.Url.Contains("/Log")).Select(e => e.PrepareRequestModel(_logOption).DeserializeRequestModel().ToDocument());

            await _requestRepository.InsertManyAsync(requestDocuments);
        }

        public async Task RetentionAsync(DateTime dateTime)
        {
            await _requestRepository.DeleteManyAsync(x => x.CreatedDateUtc <= dateTime);
        }
    }
}
