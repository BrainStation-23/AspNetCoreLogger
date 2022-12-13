using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
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
            ILogger<SqlLogRepository> logger,IOptions<LogOption>logOptions)
        {
            _dapper = dapper;
            _logger = logger;
            _logOptions = logOptions.Value;
        }

        public async Task AddAsync(SqlModel sqlModel)
        {
            List<string> ignoreColumns = _logOptions.Log.Sql.EnableIgnore?_logOptions.Log.Sql.IgnoreColumns:new List<string> { };
            List<string> maskColumns = _logOptions.Log.Sql.EnableMask ? _logOptions.Log.Sql.MaskColumns : new List<string> { };
            if (sqlModel.Url.Contains("/Log/", StringComparison.InvariantCultureIgnoreCase))
                return;

            var createdDateUtc =ignoreColumns.Contains("CreatedDateUtc")?null: maskColumns.Contains("CreatedDateUtc") ?"****": DateTime.UtcNow.ToString();

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
                //toFilter was not working
                foreach (var property in ignoreColumns)
                {
                    DropProperty(sqlModel, property);
                }
                foreach (var property in maskColumns)
                {
                    MaskProperty(sqlModel, property);
                }
                var model = new
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
                    Protocol = sqlModel.Proctocol,
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
                };
                
                await connection.ExecuteAsync(query, model);
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

        public void DropProperty(SqlModel obj, string propertyName)
        {

            obj.GetType().GetProperty(propertyName)?.SetValue(obj, null);
            
        }
        public void MaskProperty(object obj, string propertyName)
        {
            obj.GetType().GetProperty(propertyName)?.SetValue(obj, "****");
            
        }
    }
}
