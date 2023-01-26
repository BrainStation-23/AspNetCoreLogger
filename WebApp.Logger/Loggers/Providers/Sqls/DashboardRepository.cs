using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Common.Serialize;
using WebApp.Logger.DashboardModels;
using WebApp.Logger.Extensions;
using WebApp.Logger.Loggers.Repositories;
using WebApp.Logger.Providers.Sqls;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        
        public async Task<dynamic> GetAuditPageByDateAsync(DateTime startDateTime, DateTime endDateTime, DapperPager pager)
        {
            dynamic auditLogs;
            var query = @"SELECT * FROM [dbo].[AuditTrailLogs]
                              WHERE
                             [DateTime] BETWEEN 
                            @startDateTime AND @endDateTime
                            ORDER BY [Id] DESC
                            OFFSET @Offset ROWS 
                            FETCH NEXT  @Next   ROWS ONLY";

            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    object param = new
                    {
                        startDateTime = startDateTime.ToString("yyyy-MM-dd hh:mm:ss"),
                        endDateTime = endDateTime.ToString("yyyy-MM-dd hh:mm:ss"),
                        Offset = pager.Offset,
                        Next = pager.Next
                    };
                    var auditLogsEntities = await connection.QueryAsync(query, param);
                    var auditLogUnescapeString = JsonSerializeExtentions.ToJson(auditLogsEntities).JsonUnescaping();
                    auditLogs = JArray.Parse(auditLogUnescapeString);
                }

                return auditLogs;
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(DashboardRepository), exception);
                throw;
            }
        }
        
        public async Task<dynamic> GetAuditPageByDateAsync(string TraceId)
        {
            dynamic auditLogs;
            var query = @"SELECT * FROM [dbo].[AuditTrailLogs]
                          WHERE
                          [TraceId]=@TraceId
                          ORDER BY [Id] DESC";

            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    object param = new
                    {
                        TraceId = TraceId
                    };
                    var auditLogsEntities = await connection.QueryAsync(query, param);
                    var auditLogUnescapeString = JsonSerializeExtentions.ToJson(auditLogsEntities).JsonUnescaping();
                    auditLogs = JArray.Parse(auditLogUnescapeString);
                }

                return auditLogs;
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(DashboardRepository), exception);
                throw;
            }
        }
        
        public async Task<dynamic> GetRequestPageByDateAsync(DateTime startDateTime, DateTime endDateTime, DapperPager pager)
        {
            dynamic requestLogs;

            var query = @"SELECT * FROM [dbo].[RouteLogs]
                              WHERE
                             [CreatedDateUtc] BETWEEN 
                            @startDateTime AND @endDateTime
                            ORDER BY [Id] DESC
                            OFFSET @Offset ROWS 
                            FETCH NEXT  @Next   ROWS ONLY";

            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    object param = new
                    {
                        startDateTime = startDateTime.ToString("yyyy-MM-dd hh:mm:ss"),
                        endDateTime = endDateTime.ToString("yyyy-MM-dd hh:mm:ss"),
                        Offset = pager.Offset,
                        Next = pager.Next
                    };
                    var requestLogsEntities = await connection.QueryAsync(query, param);
                    var requestLogsUnescapeString = JsonSerializeExtentions.ToJson(requestLogsEntities).JsonUnescaping();
                    requestLogs = JArray.Parse(requestLogsUnescapeString);
                }

                return requestLogs;
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(DashboardRepository), exception);
                throw;
            }
        }
        
        public async Task<dynamic> GetRequestPageByDateAsync(string TraceId)
        {
            dynamic requestLogs;

            var query = @"SELECT * FROM [dbo].[RouteLogs]
                          WHERE
                          [TraceId]=@TraceId
                          ORDER BY [Id] DESC";

            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    object param = new
                    {
                        TraceId = TraceId
                    };
                    var requestLogsEntities = await connection.QueryAsync(query, param);
                    var requestLogsUnescapeString = JsonSerializeExtentions.ToJson(requestLogsEntities).JsonUnescaping();
                    requestLogs = JArray.Parse(requestLogsUnescapeString);
                }

                return requestLogs;
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(DashboardRepository), exception);
                throw;
            }
        }
        
        public async Task<dynamic> GetSqlPageByDateAsync(DateTime startDateTime, DateTime endDateTime, DapperPager pager)
        {
            dynamic sqlLogs;

            var query = @"SELECT * FROM [dbo].[SqlLogs]
                              WHERE
                             [CreatedDateUtc] BETWEEN 
                            @startDateTime AND @endDateTime
                            ORDER BY [Id] DESC
                            OFFSET @Offset ROWS 
                            FETCH NEXT  @Next   ROWS ONLY";

            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    object param = new
                    {
                        startDateTime = startDateTime.ToString("yyyy-MM-dd hh:mm:ss"),
                        endDateTime = endDateTime.ToString("yyyy-MM-dd hh:mm:ss"),
                        Offset = pager.Offset,
                        Next = pager.Next
                    };
                    var sqlLogsEntities = await connection.QueryAsync(query, param);
                    var sqlLogUnescapeString = JsonSerializeExtentions.ToJson(sqlLogsEntities).JsonUnescaping();
                    sqlLogs = JArray.Parse(sqlLogUnescapeString);
                }

                return sqlLogs;
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(DashboardRepository), exception);
                throw;
            }
        }
        
        public async Task<dynamic> GetSqlPageByDateAsync(string TraceId)
        {
            dynamic sqlLogs;

            var query = @"SELECT * FROM [dbo].[SqlLogs]
                          WHERE
                          [TraceId]=@TraceId
                          ORDER BY [Id] DESC";

            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    object param = new
                    {
                        TraceId = TraceId
                    };
                    var sqlLogsEntities = await connection.QueryAsync(query, param);
                    var sqlLogUnescapeString = JsonSerializeExtentions.ToJson(sqlLogsEntities).JsonUnescaping();
                    sqlLogs = JArray.Parse(sqlLogUnescapeString);
                }

                return sqlLogs;
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(DashboardRepository), exception);
                throw;
            }
        }
        
        public async Task<dynamic> GetErrorPageByDateAsync(DateTime startDateTime, DateTime endDateTime, DapperPager pager)
        {
            dynamic logs;
            /*var query = @"SELECT 
                                  *
                          FROM [dbo].[ExceptionLogs]
                              WHERE
                             [CreatedDateUtc] BETWEEN 
                            @startDateTime AND @endDateTime
                            ORDER BY [Id] DESC
                            OFFSET @Offset ROWS 
                            FETCH NEXT  @Next   ROWS ONLY";*/
            var query = @"SELECT 
                                  [Id]
                                 ,[UserId]
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
                                 ,[ErrorCode]
                                 ,[Scheme]
                                 ,[TraceId]
                                 ,[Protocol]
                                 ,[Errors]
                                 ,[StatusCode]
                                ,[AppStatusCode]
                                 ,[CreatedDateUtc]
                          FROM [dbo].[ExceptionLogs]
                              WHERE
                             [CreatedDateUtc] BETWEEN 
                            @startDateTime AND @endDateTime
                            ORDER BY [Id] DESC
                            OFFSET @Offset ROWS 
                            FETCH NEXT  @Next   ROWS ONLY";

            //confirm - ,[MessageDetails],[Message]

            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    object param = new
                    {
                        startDateTime = startDateTime.ToString("yyyy-MM-dd hh:mm:ss"),
                        endDateTime = endDateTime.ToString("yyyy-MM-dd hh:mm:ss"),
                        Offset = pager.Offset,
                        Next = pager.Next
                    };
                    var errorrLogsEntities = await connection.QueryAsync(query, param);
                    var errorLogUnescapeString = JsonSerializeExtentions.ToJson(errorrLogsEntities).JsonUnescaping();
                    logs = JArray.Parse(errorLogUnescapeString);
                }

                return logs;


            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(DashboardRepository), exception);
                throw;
            }
        }
        
        public async Task<dynamic> GetErrorPageByDateAsync(string TraceId)
        {
            dynamic logs;
            /*var query = @"SELECT 
                                  *
                          FROM [dbo].[ExceptionLogs]
                              WHERE
                             [CreatedDateUtc] BETWEEN 
                            @startDateTime AND @endDateTime
                            ORDER BY [Id] DESC
                            OFFSET @Offset ROWS 
                            FETCH NEXT  @Next   ROWS ONLY";*/
            var query = @"SELECT 
                                  [Id]
                                 ,[UserId]
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
                                 ,[ErrorCode]
                                 ,[Scheme]
                                 ,[TraceId]
                                 ,[Protocol]
                                 ,[Errors]
                                 ,[StatusCode]
                                ,[AppStatusCode]
                                 ,[CreatedDateUtc]
                          FROM [dbo].[ExceptionLogs]
                               WHERE
                          [TraceId]=@TraceId
                          ORDER BY [Id] DESC";

            //confirm - ,[MessageDetails],[Message]

            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    object param = new
                    {
                        TraceId = TraceId
                    };
                    var errorrLogsEntities = await connection.QueryAsync(query, param);
                    var errorLogUnescapeString = JsonSerializeExtentions.ToJson(errorrLogsEntities).JsonUnescaping();
                    logs = JArray.Parse(errorLogUnescapeString);
                }

                return logs;


            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(DashboardRepository), exception);
                throw;
            }
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
                using (var connection = _dapper.CreateConnection())
                {
                    List<TopRequestDashboardModel> topRequests = (await connection.QueryAsync<TopRequestDashboardModel>(TopRequestsQuery)).ToList();

                    return topRequests;
                }


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

        public async Task<dynamic> GetLogCountSummaryAsync()
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
                    LogsCountSummary logsCountSummary = (await connection.QueryAsync<LogsCountSummary>(LogsCountSummaryQuery)).FirstOrDefault();

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
