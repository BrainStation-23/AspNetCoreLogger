using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using WebApp.Common.Serialize;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Loggers.Repositories
{
    public class SqlFileLogRepository : ISqlLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<SqlLogRepository> _logger;

        public SqlFileLogRepository(DapperContext dapper,
            ILogger<SqlLogRepository> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }

        public async Task AddAsync(SqlModel sqlModel)
        {
            if (sqlModel.Url.Contains("/Log/"))
                return;

            var createdDateUtc = DateTime.UtcNow.ToString();
            var query = @"INSERT INTO [dbo].[SqlLogs]
                            ([UserId]
                            ,[ApplicationName]
                            ,[IpAddress]
                            ,[Version]
                            ,[Host]
                            ,[Url]
                            ,[Source]
                            ,[Scheme]
                            ,[TraceId]
                            ,[Protocol]
                            ,[UrlReferrer]
                            ,[Area]
                            ,[ControllerName]
                            ,[ActionName]
                            ,[ClassName]
                            ,[MethodName]
                            ,[QueryType]
                            ,[Query]
                            ,[Response]  
                            ,[Duration]
                            ,[Message]    
                            ,[Connection]
                            ,[Command]
                            ,[Event]
                            ,[CreatedDateUtc] )
                         VALUES
                            ( @UserId
                            , @ApplicationName
                            , @IpAddress
                            , @Version
                            , @Host
                            , @Url
                            , @Source                            
                            , @Scheme
                            , @TraceId
                            , @Protocol
                            , @UrlReferrer
                            , @Area
                            , @ControllerName
                            , @ActionName
                            , @ClassName
                            , @MethodName
                            , @QueryType
                            , @Query
                            , @Response                            
                            , @Duration
                            , @Message
                            , @Connection
                            , @Command
                            , @Event
                            , @CreatedDateUtc)";

            try
            {
                using var connection = _dapper.CreateConnection();
                await connection.ExecuteAsync(query, new
                {
                    UserId = sqlModel.UserId,
                    ApplicationName = sqlModel.ApplicationName,
                    IpAddress = sqlModel.IpAddress,
                    Version = sqlModel.Version,
                    Host = sqlModel.Host,
                    Url = sqlModel.Url,
                    Source = sqlModel.Source,
                    Scheme = sqlModel.Scheme,
                    TraceId = sqlModel.TraceId,
                    Protocol = sqlModel.Protocol,
                    UrlReferrer = sqlModel.UrlReferrer,
                    Area = sqlModel.Area,
                    ControllerName = sqlModel.ControllerName,
                    ActionName = sqlModel.ActionName,
                    ClassName = sqlModel.ClassName,
                    MethodName = sqlModel.MethodName,
                    QueryType = sqlModel.QueryType,
                    Query = sqlModel.Query,
                    Response = sqlModel.Response,
                    Duration = sqlModel.Duration,
                    Message = sqlModel.Message,
                    Connection = sqlModel.Connection,
                    Command = sqlModel.Command,
                    Event = sqlModel.Event,
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
            dynamic sqlLogs;
            var query = @"SELECT * FROM [dbo].[SqlLogs]
                            ORDER BY [Id] DESC
                            OFFSET @Offset ROWS 
                            FETCH NEXT  @Next   ROWS ONLY";

            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    var sqlLogsEntities = await connection.QueryAsync(query, pager);
                    var sqlLogUnescapeString = sqlLogsEntities.ToJson().JsonUnescaping();
                    sqlLogs = JArray.Parse(sqlLogUnescapeString);
                }

                return sqlLogs;
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(SqlLogRepository), exception);
                throw;
            }
        }
    }
}
