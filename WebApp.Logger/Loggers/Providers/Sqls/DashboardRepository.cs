using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Logger.DashboardModels;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Loggers.Providers.Sqls
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<DashboardRepository> _logger;
        public DashboardRepository(DapperContext dapper,
            ILogger<DashboardRepository> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }

        public async Task<dynamic> GetTopRequestsAsync()
        {
            var TopRequestsQuery = @"SELECT Top 5 COUNT(*) AS Frequency
                                    ,[Url]
                                    ,[RequestMethod]
                                    ,[ApplicationName]
                                    ,[ActionName]
                                    ,[ControllerName]
                                     FROM [DotnetLoggerWrapper].[dbo].[RouteLogs]
                                     WHERE (Url is not null ) AND (Url <> '')
                                     GROUP BY [Url]
                                    ,[RequestMethod]
                                    ,[ApplicationName]
                                    ,[ActionName]
                                    ,[ControllerName]
                                     ORDER BY COUNT(*) DESC";
            try
            {
                using var connection = _dapper.CreateConnection();
                List<TopRequestDashboardModel> topRequests = (await connection.QueryAsync<TopRequestDashboardModel>(TopRequestsQuery)).ToList();

                return topRequests;


            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(DashboardRepository), exception);
                throw;
            }

        }

        public async Task<dynamic> GetTopExceptionAsync()
        {
            var TopExceptionsQuery = @"SELECT Top 5 COUNT(*) AS Frequency
                                    ,[RequestMethod]
                                    ,[Url]
                                    ,[ActionName]
                                    ,[ControllerName]
                                    ,[ErrorCode]
                                    ,[StatusCode]
                                     FROM [DotnetLoggerWrapper].[dbo].[ExceptionLogs]
                                     WHERE (Url is not null ) AND (Url <> '')
                                     GROUP BY [RequestMethod]
                                    ,[Url]
                                    ,[ActionName]
                                    ,[ControllerName]
                                    ,[ErrorCode]
                                    ,[StatusCode]
                                     ORDER BY COUNT(*) DESC";
            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    List<TopExceptionDashboardModel> topExceptions = (await connection.QueryAsync<TopExceptionDashboardModel>(TopExceptionsQuery)).ToList();

                    return topExceptions;
                }

            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(DashboardRepository), exception);
                throw;
            }

        }

        public async Task<dynamic> GetSummaryAsync()
        {
            var LogsCountSummaryQuery = @"DECLARE @LogCountSummary As table(
                                        TotalAuditLogs BIGINT,
                                        TotalSqlLogs BIGINT,
                                        TotalErrorLogs BIGINT,
                                        TotalRequestLogs BIGINT
                                        )

                                        DECLARE @sqlCount AS BIGINT =(select COUNT(*) FROM [DotnetLoggerWrapper].[dbo].[SqlLogs])
                                        DECLARE @routeCount AS BIGINT =(select COUNT(*) FROM [DotnetLoggerWrapper].[dbo].[RouteLogs])
                                        DECLARE @exceptionCount AS BIGINT =(select COUNT(*) FROM [DotnetLoggerWrapper].[dbo].[ExceptionLogs])
                                        DECLARE @auditCount AS BIGINT =(select COUNT(*) FROM [DotnetLoggerWrapper].[dbo].[AuditTrailLogs])

                                        INSERT INTO @LogCountSummary
                                        VALUES(@auditCount, @sqlCount, @exceptionCount, @routeCount)

                                        SELECT * FROM @LogCountSummary";
            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    SummaryDashboardModel logsCountSummary = (await connection.QueryAsync<SummaryDashboardModel>(LogsCountSummaryQuery)).FirstOrDefault();

                    return logsCountSummary;
                }

            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(DashboardRepository), exception);
                throw;
            }

        }

        public async Task<dynamic> GetSlowestRequestAsync()
        {

            var SlowestRequestQuery = @"SELECT Top 1 *
                                        FROM [DotnetLoggerWrapper].[dbo].[RouteLogs]
                                        Where (Url is not null ) AND (Url <> '')
                                        ORDER BY Duration DESC";

            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    RequestLogDashboardModel slowestRequests = (await connection.QueryAsync<RequestLogDashboardModel>(SlowestRequestQuery)).FirstOrDefault();

                    return slowestRequests;
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(DashboardRepository), exception);
                throw;
            }

        }

        public async Task<dynamic> GetFastestRequestAsync()
        {

            var FastestRequestQuery = @"SELECT Top 1 *
                                        FROM [DotnetLoggerWrapper].[dbo].[RouteLogs]
                                        Where (Url is not null ) AND (Url <> '')
                                        ORDER BY Duration ASC";

            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    RequestLogDashboardModel fastestRequests = (await connection.QueryAsync<RequestLogDashboardModel>(FastestRequestQuery)).FirstOrDefault();

                    return fastestRequests;
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(DashboardRepository), exception);
                throw;
            }

        }
    }
}
