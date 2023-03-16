using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Logger.Contracts;
using WebApp.Logger.Extensions;
using WebApp.Logger.Models;

namespace WebApp.Logger.Providers.Sqls.Repos
{
    public class RequestLogRepository : IRequestLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<RequestLogRepository> _logger;
        private readonly string _tableName;

        public RequestLogRepository(DapperContext dapper,
            ILogger<RequestLogRepository> logger)
        {
            _dapper = dapper;
            _logger = logger;
            _tableName = SqlVariable.RequestTableName;
        }

        public async Task AddAsync(RequestModel requestModel)
        {
            if (requestModel.Url.Contains("/Log/", StringComparison.InvariantCultureIgnoreCase))
                return;

            var createdDateUtc = DateTime.UtcNow.ToString();
            var query = $@"INSERT INTO {_tableName}
                               ([UserId]
                               ,[ApplicationName]
                               ,[IpAddress]
                               ,[Version]
                               ,[Host]
                               ,[Url]
                               ,[Source]
                               ,[Form]
                               ,[Body]
                               ,[Response]
                               ,[RequestHeaders]
                               ,[ResponseHeaders]
                               ,[Scheme]
                               ,[TraceId]
                               ,[Protocol]
                               ,[UrlReferrer]
                               ,[Area]
                               ,[ControllerName]
                               ,[ActionName]
                               ,[Duration]
                               ,[StatusCode]
                               ,[AppStatusCode]
                               ,[CreatedDateUtc] )
                         VALUES
                               ( @UserId
                               , @ApplicationName
                               , @IpAddress
                               , @Version
                               , @Host
                               , @Url
                               , @Source
                               , @Form
                               , @Body
                               , @Response
                               , @RequestHeaders
                               , @ResponseHeaders
                               , @Scheme
                               , @TraceId
                               , @Protocol
                               , @UrlReferrer
                               , @Area
                               , @ControllerName
                               , @ActionName
                               , @Duration
                               , @StatusCode
                               , @AppStatusCode
                               , @CreatedDateUtc)";

            try
            {
                using var connection = _dapper.CreateConnection();
                await connection.ExecuteAsync(query, new
                {
                    requestModel.UserId,
                    requestModel.ApplicationName,
                    requestModel.IpAddress,
                    requestModel.Version,
                    requestModel.Host,
                    requestModel.Url,
                    requestModel.Source,
                    requestModel.Form,
                    requestModel.Body,
                    requestModel.Response,
                    requestModel.RequestHeaders,
                    requestModel.ResponseHeaders,
                    requestModel.Scheme,
                    requestModel.TraceId,
                    Protocol = requestModel.Proctocol,
                    requestModel.UrlReferrer,
                    requestModel.Area,
                    requestModel.ControllerName,
                    requestModel.ActionName,
                    requestModel.Duration,
                    StatusCode = ((int)requestModel.StatusCode).ToString(),
                    requestModel.AppStatusCode,
                    CreatedDateUtc = createdDateUtc
                });
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(RequestLogRepository), exception);
            }
        }

        public async Task AddAsync(List<RequestModel> requestModels)
        {
            var requestLogs = new List<object>();
            var createdDateUtc = DateTime.UtcNow.ToString();
            var query = $@"INSERT INTO {_tableName}
                               ([UserId]
                               ,[ApplicationName]
                               ,[IpAddress]
                               ,[Version]
                               ,[Host]
                               ,[Url]
                               ,[Source]
                               ,[Form]
                               ,[Body]
                               ,[Response]
                               ,[RequestHeaders]
                               ,[ResponseHeaders]
                               ,[Scheme]
                               ,[TraceId]
                               ,[Protocol]
                               ,[UrlReferrer]
                               ,[Area]
                               ,[ControllerName]
                               ,[ActionName]
                               ,[Duration]
                               ,[StatusCode]
                               ,[AppStatusCode]
                               ,[Session]
                               ,[RequestMethod]
                               ,[RequestLength]
                               ,[ResponseLength]
                               ,[IsHttps]
                               ,[CorrelationId]
                               ,[CreatedDateUtc])
                         VALUES
                               ( @UserId
                               , @ApplicationName
                               , @IpAddress
                               , @Version
                               , @Host
                               , @Url
                               , @Source
                               , @Form
                               , @Body
                               , @Response
                               , @RequestHeaders
                               , @ResponseHeaders
                               , @Scheme
                               , @TraceId
                               , @Protocol
                               , @UrlReferrer
                               , @Area
                               , @ControllerName
                               , @ActionName
                               , @Duration
                               , @StatusCode
                               , @AppStatusCode
                               , @Session
                               , @RequestMethod
                               , @RequestLength
                               , @ResponseLength
                               , @IsHttps
                               , @CorrelationId
                               , @CreatedDateUtc)";

            requestModels.ForEach(requestModel =>
            {
                requestLogs.Add(new
                {
                    requestModel.UserId,
                    requestModel.ApplicationName,
                    requestModel.IpAddress,
                    requestModel.Version,
                    requestModel.Host,
                    requestModel.Url,
                    requestModel.Source,
                    requestModel.Form,
                    requestModel.Body,
                    requestModel.Response,
                    requestModel.RequestHeaders,
                    requestModel.ResponseHeaders,
                    requestModel.Scheme,
                    requestModel.TraceId,
                    Protocol = requestModel.Proctocol,
                    requestModel.UrlReferrer,
                    requestModel.Area,
                    requestModel.ControllerName,
                    requestModel.ActionName,
                    requestModel.Duration,
                    StatusCode = ((int)requestModel.StatusCode).ToString(),
                    requestModel.AppStatusCode,
                    Session = string.Empty,
                    requestModel.RequestMethod,
                    requestModel.RequestLength,
                    requestModel.ResponseLength,
                    requestModel.IsHttps,
                    requestModel.CorrelationId,
                    CreatedDateUtc = createdDateUtc
                });
            });

            using var connection = _dapper.CreateConnection();
            await connection.ExecuteAsync(query, requestLogs);
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            dynamic routeLogs;
            var query = $@"SELECT * FROM {_tableName}
                            ORDER BY [Id] DESC
                            OFFSET @Offset ROWS 
                            FETCH NEXT  @Next   ROWS ONLY";

            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    var routeLogsEntities = await connection.QueryAsync(query, pager);
                    var routeLogUnescapeString = routeLogsEntities.ToJson().JsonUnescaping();
                    routeLogs = JArray.Parse(routeLogUnescapeString);
                }

                return routeLogs;
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(RequestLogRepository), exception);
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
