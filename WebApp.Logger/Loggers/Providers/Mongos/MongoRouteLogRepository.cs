﻿using Microsoft.Extensions.Logging;
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

        public MongoRouteLogRepository(DapperContext dapper,
            ILogger<MongoRouteLogRepository> logger,
            IMongoRepository<RequestLogDocument> RequestRepository)
        {
            _dapper = dapper;
            _logger = logger;
            _RequestRepository = RequestRepository;
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            //Expression<Func<RequestLogDocument, bool>> isDate = s => s.DateTime != null;
            //var filter = Builders<RequestLogDocument>.Filter.Where(x => x.DateTime != null);
            return await _RequestRepository.GetPageAsync(pager);
        }

        public async Task AddAsync(RequestModel requestModel)
        {
            if (requestModel.Url.Contains("/Log/"))
                return;

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