using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Logger.Contracts;
using WebApp.Logger.DashboardModels;
using WebApp.Logger.Loggers;

namespace WebApp.Logger.Providers.Sqls.Repos
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
            var topRequestsQuery = $@"SELECT Top 5 COUNT(*) AS Frequency
                                        ,[Url]
                                        ,[RequestMethod]
                                        ,[ApplicationName]
                                        ,[ActionName]
                                        ,[ControllerName]
                                     FROM {SqlVariable.RequestTableName}
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
                List<TopRequestDashboardModel> topRequests = (await connection.QueryAsync<TopRequestDashboardModel>(topRequestsQuery)).ToList();

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
            var TopExceptionsQuery = $@"SELECT Top 5 COUNT(*) AS Frequency
                                        ,[RequestMethod]
                                        ,[Url]
                                        ,[ActionName]
                                        ,[ControllerName]
                                        ,[ErrorCode]
                                        ,[StatusCode]
                                     FROM {SqlVariable.ErrorTableName}
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
            var summaryQuery = $@"DECLARE @LogCountSummary As table(
                                            TotalAuditLogs BIGINT,
                                            TotalSqlLogs BIGINT,
                                            TotalErrorLogs BIGINT,
                                            TotalRequestLogs BIGINT
                                        )

                                        DECLARE @sqlCount AS BIGINT =(select COUNT(*) FROM {SqlVariable.SqlTableName})
                                        DECLARE @routeCount AS BIGINT =(select COUNT(*) FROM {SqlVariable.RequestTableName})
                                        DECLARE @exceptionCount AS BIGINT =(select COUNT(*) FROM {SqlVariable.ErrorTableName})
                                        DECLARE @auditCount AS BIGINT =(select COUNT(*) FROM {SqlVariable.AuditTableName})

                                        INSERT INTO @LogCountSummary
                                        VALUES(@auditCount, @sqlCount, @exceptionCount, @routeCount)

                                        SELECT * FROM @LogCountSummary";
            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    SummaryDashboardModel summary = (await connection.QueryAsync<SummaryDashboardModel>(summaryQuery)).FirstOrDefault();

                    return summary;
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

            var slowestRequestQuery = $@"SELECT Top 1 *
                                        FROM {SqlVariable.RequestTableName}
                                        Where (Url is not null ) AND (Url <> '')
                                        ORDER BY Duration DESC";

            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    RequestLogDashboardModel slowestRequests = (await connection.QueryAsync<RequestLogDashboardModel>(slowestRequestQuery)).FirstOrDefault();

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

            var fastestRequestQuery = $@"SELECT Top 1 *
                                        FROM {SqlVariable.RequestTableName}
                                        Where (Url is not null ) AND (Url <> '')
                                        ORDER BY Duration ASC";

            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    RequestLogDashboardModel fastestRequests = (await connection.QueryAsync<RequestLogDashboardModel>(fastestRequestQuery)).FirstOrDefault();

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
