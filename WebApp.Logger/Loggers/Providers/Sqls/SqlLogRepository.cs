using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Common.Serialize;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Loggers.Repositories
{
    public class SqlLogRepository : ISqlLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<SqlLogRepository> _logger;
        private readonly LogOption _logOptions;
        public SqlLogRepository(DapperContext dapper,
            ILogger<SqlLogRepository> logger,
            IOptions<LogOption> logOptions)
        {
            _dapper = dapper;
            _logger = logger;
            _logOptions = logOptions.Value;
        }

        public async Task AddAsync(SqlModel sqlModel)
        {
            if (LogOptionExtension.SkipSqlLog(sqlModel, _logOptions))
                return;

            sqlModel = sqlModel.PrepareSqlModel(_logOptions);

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
                sqlModel = sqlModel.SerializeSqlModel();
                await connection.ExecuteAsync(query, sqlModel);
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(RouteLogRepository), exception);
            }
        }

        public async Task AddAsync(List<SqlModel> sqlModels)
        {
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
            
            sqlModels=sqlModels.Select(sqlModel =>sqlModel.PrepareSqlModel(_logOptions).SerializeSqlModel()).ToList();
            try
            {
                using var connection = _dapper.CreateConnection();
                await connection.ExecuteAsync(query, sqlModels);
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

        public async Task RetentionAsync(DateTime dateTime)
        {
            string date = dateTime.ToString();//"2023-01-04 06:11:12.2333333"
            var query = $"delete from [dbo].[SqlLogs] where CreatedDateUtc <= '{date}'";
            using var connection = _dapper.CreateConnection();
            await connection.ExecuteAsync(query);
        }
    }
}
