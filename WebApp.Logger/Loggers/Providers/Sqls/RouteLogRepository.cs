using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using WebApp.Common.Serialize;
using WebApp.Logger.Providers.Sqls;
using WebApp.Logger.Models;

namespace WebApp.Logger.Loggers.Repositories
{
    public class RouteLogRepository : IRouteLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<RouteLogRepository> _logger;

        public RouteLogRepository(DapperContext dapper,
            ILogger<RouteLogRepository> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }


        public async Task AddAsync(RequestModel requestModel)
        {
            if (requestModel.Url.Contains("/Log/"))
                return;

            var createdDateUtc = DateTime.UtcNow.ToString();
            var query = @"INSERT INTO [dbo].[RouteLogs]
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
                               ,[ExecutionDuration]
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
                               , @ExecutionDuration
                               , @StatusCode
                               , @AppStatusCode
                               , @CreatedDateUtc)";

            try
            {
                using var connection = _dapper.CreateConnection();
                await connection.ExecuteAsync(query, new
                {
                    UserId = requestModel.UserId,
                    ApplicationName = requestModel.Application,
                    IpAddress = requestModel.IpAddress,
                    Version = requestModel.Version,
                    Host = requestModel.Host,
                    Url = requestModel.Url,
                    Source = requestModel.Source,
                    Form = requestModel.Form,
                    Body = requestModel.Body,
                    Response = requestModel.Response,
                    RequestHeaders = requestModel.RequestHeaders,
                    ResponseHeaders = requestModel.ResponseHeaders,
                    Scheme = requestModel.Scheme,
                    TraceId = requestModel.TraceId,
                    Protocol = requestModel.Proctocol,
                    UrlReferrer = requestModel.UrlReferrer,
                    Area = requestModel.Area,
                    ControllerName = requestModel.ControllerName,
                    ActionName = requestModel.ActionName,
                    ExecutionDuration = requestModel.ExecutionDuration,
                    StatusCode = ((int)requestModel.StatusCode).ToString(),
                    AppStatusCode = requestModel.AppStatusCode,
                    CreatedDateUtc = createdDateUtc
                });
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(RouteLogRepository), exception);
            }
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            dynamic routeLogs;
            var query = @"SELECT * FROM [dbo].[RouteLogs]
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
                _logger.LogError(nameof(RouteLogRepository), exception);
                throw;
            }
        }
    }
}
