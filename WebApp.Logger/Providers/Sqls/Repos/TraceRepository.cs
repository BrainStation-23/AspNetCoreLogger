using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Logger.Contracts;
using WebApp.Logger.Extensions;
using WebApp.Logger.Loggers;
using WebApp.Logger.Models;

namespace WebApp.Logger.Providers.Sqls.Repos
{
    public class TraceRepository : ITraceRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<TraceRepository> _logger;
        private readonly LogOption _logOptions;
        private readonly string _tableName;

        public TraceRepository(DapperContext dapper,
            ILogger<TraceRepository> logger,
            IOptions<LogOption> logOptions)
        {
            _dapper = dapper;
            _logger = logger;
            _logOptions = logOptions.Value;
            _tableName = SqlVariable.TraceTableName;
        }

        public async Task AddAsync(TraceModel model)
        {
            var query = $@"INSERT INTO {_tableName}
                            ([UserId]
                            ,[ApplicationName]
                            ,[IpAddress]
                            ,[Url]
                            ,[Source]
                            ,[TraceId]
                            ,[Duration]
                            ,[Trace]    
                            ,[CreatedDateUtc] )
                         VALUES
                            ( @UserId
                            , @ApplicationName
                            , @IpAddress
                            , @Url
                            , @Source                            
                            , @TraceId
                            , @Duration
                            , @Trace
                            , @CreatedDateUtc)";

            try
            {
                using var connection = _dapper.CreateConnection();
                await connection.ExecuteAsync(query, model);
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(RequestLogRepository), exception);
            }
        }

        public async Task AddAsync(List<TraceModel> models)
        {
            var query = $@"INSERT INTO {_tableName}
                            ([UserId]
                            ,[ApplicationName]
                            ,[IpAddress]
                            ,[Url]
                            ,[Source]
                            ,[TraceId]
                            ,[Duration]
                            ,[Trace]    
                            ,[CreatedDateUtc] )
                         VALUES
                            ( @UserId
                            , @ApplicationName
                            , @IpAddress
                            , @Url
                            , @Source                            
                            , @TraceId
                            , @Duration
                            , @Trace
                            , @CreatedDateUtc)";

            try
            {
                using var connection = _dapper.CreateConnection();
                await connection.ExecuteAsync(query, models);
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(RequestLogRepository), exception);
            }
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            dynamic traces;
            var query = $@"SELECT * FROM {_tableName}
                            ORDER BY [Id] DESC
                            OFFSET @Offset ROWS 
                            FETCH NEXT  @Next   ROWS ONLY";

            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    var traceEntities = await connection.QueryAsync(query, pager);
                    var traceUnescapeString = traceEntities.ToJson().JsonUnescaping();
                    traces = JArray.Parse(traceUnescapeString);
                }

                return traces;
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(TraceRepository), exception);
                throw;
            }
        }

        public async Task RetentionAsync(DateTime dateTime)
        {
            string date = dateTime.ToString();//"2023-01-04 06:11:12.2333333"
            var query = $"delete from {_tableName} where CreatedDateUtc <= '{date}'";
            using var connection = _dapper.CreateConnection();
            await connection.ExecuteAsync(query);
        }
    }
}
