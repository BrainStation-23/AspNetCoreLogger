using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Logger.Extensions;
using WebApp.Logger.Loggers.Providers.Sqls;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Loggers.Repositories
{
    public class ErrorLogRepository : IErrorLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<ErrorLogRepository> _logger;
        private readonly LogOption _logOptions;
        private readonly string _tableName;

        public ErrorLogRepository(DapperContext dapper,
            ILogger<ErrorLogRepository> logger, IOptions<LogOption> logOptions)
        {
            _dapper = dapper;
            _logger = logger;
            _logOptions = logOptions.Value;
            _tableName = SqlVariable.ErrorTableName;
        }

        public async Task AddAsync(ErrorModel errorModel)
        {
            if (LogOptionExtension.SkipErrorLog(errorModel, _logOptions))
                return;

            errorModel = errorModel.PrepareErrorModel(_logOptions);

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
                               ,[ErrorCode]
                               ,[Scheme]
                               ,[TraceId]
                               ,[Protocol]
                               ,[Errors]
                               ,[StatusCode]
                               ,[AppStatusCode]
                               ,[Message]
                               ,[MessageDetails]
                               ,[StackTrace]
                               ,[CreatedDateUtc]
                               ,[ControllerName]
                               ,[ActionName]
                               ,[Duration]
                               ,[RequestMethod])
                            
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
                               , @ErrorCode
                               , @Scheme
                               , @TraceId
                               , @Protocol
                               , @Errors
                               , @StatusCode
                               , @AppStatusCode
                               , @Message
                               , @MessageDetails
                               , @StackTrace
                               , @CreatedDateUtc
                               , @ControllerName
                               , @ActionName
                               , @Duration
                               , @RequestMethod)";

            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    await connection.ExecuteAsync(query, new
                    {
                        UserId = errorModel.UserId,
                        ApplicationName = errorModel.ApplicationName,
                        ControllerName = errorModel.ControllerName,
                        ActionName = errorModel.ActionName,
                        Duration = errorModel.Duration,
                        RequestMethod = errorModel.RequestMethod,
                        IpAddress = errorModel.IpAddress,
                        Version = errorModel.Version,
                        Host = errorModel.Host,
                        Url = errorModel.Url,
                        Source = errorModel.Source,
                        Form = errorModel.Form,
                        Body = errorModel.Body,
                        Response = errorModel.Response,
                        RequestHeaders = errorModel.RequestHeaders,
                        ResponseHeaders = errorModel.ResponseHeaders,
                        ErrorCode = errorModel.ErrorCode,
                        Scheme = errorModel.Scheme,
                        TraceId = errorModel.TraceId,
                        Protocol = errorModel.Proctocol,
                        Errors = JsonConvert.SerializeObject(errorModel.Errors),
                        StatusCode = ((int)errorModel.StatusCode).ToString(),
                        AppStatusCode = errorModel.AppStatusCode,
                        Message = errorModel.Message,
                        MessageDetails = errorModel.MessageDetails,
                        StackTrace = errorModel.StackTrace,
                        CreatedDateUtc = DateTime.UtcNow
                    });
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(ErrorLogRepository), exception);
            }
        }

        public async Task AddAsync(List<ErrorModel> errorModels)
        {
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
                               ,[ErrorCode]
                               ,[Scheme]
                               ,[TraceId]
                               ,[Protocol]
                               ,[Errors]
                               ,[StatusCode]
                               ,[AppStatusCode]
                               ,[Message]
                               ,[MessageDetails]
                               ,[StackTrace]
                               ,[CreatedDateUtc]
                               ,[ControllerName]
                               ,[ActionName]
                               ,[Duration]
                               ,[RequestMethod])
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
                               , @ErrorCode
                               , @Scheme
                               , @TraceId
                               , @Protocol
                               , @Errors
                               , @StatusCode
                               , @AppStatusCode
                               , @Message
                               , @MessageDetails
                               , @StackTrace
                               , @CreatedDateUtc
                               , @ControllerName
                               , @ActionName
                               , @Duration
                               , @RequestMethod)";

            var errorLogs = new List<object>();
            errorModels.ForEach(errorModel =>
            {
                if (LogOptionExtension.SkipErrorLog(errorModel, _logOptions))
                    return;

                errorModel = errorModel.PrepareErrorModel(_logOptions);
                errorLogs.Add(new
                {
                    UserId = errorModel.UserId,
                    ApplicationName = errorModel.ApplicationName,
                    IpAddress = errorModel.IpAddress,
                    Version = errorModel.Version,
                    Host = errorModel.Host,
                    Url = errorModel.Url,
                    Source = errorModel.Source,
                    Form = errorModel.Form,
                    Body = errorModel.Body,
                    Response = errorModel.Response,
                    RequestHeaders = errorModel.RequestHeaders,
                    ResponseHeaders = errorModel.ResponseHeaders,
                    ErrorCode = errorModel.ErrorCode,
                    Scheme = errorModel.Scheme,
                    TraceId = errorModel.TraceId,
                    Protocol = errorModel.Proctocol,
                    Errors = JsonConvert.SerializeObject(errorModel.Errors),
                    StatusCode = ((int)errorModel.StatusCode).ToString(),
                    AppStatusCode = errorModel.AppStatusCode,
                    Message = errorModel.Message,
                    MessageDetails = errorModel.MessageDetails,
                    StackTrace = errorModel.StackTrace,
                    CreatedDateUtc = DateTime.UtcNow,
                    ControllerName = errorModel.ControllerName,
                    ActionName = errorModel.ActionName,
                    Duration = errorModel.Duration,
                    RequestMethod = errorModel.RequestMethod
                });
            });

            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    await connection.ExecuteAsync(query, errorLogs);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(ErrorLogRepository), exception);
            }

        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            dynamic logs;

            var query = $@"SELECT * FROM {_tableName}
                            ORDER BY [Id] DESC
                            OFFSET @Offset ROWS 
                            FETCH NEXT @Next ROWS ONLY";

            var exceptionLogUnescapeString = string.Empty;
            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    var exceptionLogs = await connection.QueryAsync<ExceptionLogVm>(query, pager);

                    exceptionLogs.ToList().ForEach(f =>
                    {
                        f.StackTrace = JsonConvert.SerializeObject(f.StackTrace);
                    });
                    exceptionLogUnescapeString = JsonSerializeExtentions.ToJsonString(exceptionLogs);
                    var unescape = exceptionLogUnescapeString.JsonUnescaping();

                    logs = JArray.Parse(unescape);
                }

                return logs;
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(ErrorLogRepository), exception);
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

    public class ExceptionLogVm
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string ApplicationName { get; set; }
        public string IpAddress { get; set; }
        public string Version { get; set; }
        public string Host { get; set; }
        public string Url { get; set; }
        public string Source { get; set; }
        public string Form { get; set; }
        public string Body { get; set; }
        public string Response { get; set; }
        public string RequestHeaders { get; set; }
        public string ResponseHeaders { get; set; }
        public string ErrorCode { get; set; }
        public string Scheme { get; set; }
        public string TraceId { get; set; }
        public string Protocol { get; set; }
        public string Errors { get; set; }
        public string StatusCode { get; set; }
        public string AppStatusCode { get; set; }
        public string Message { get; set; }
        public string MessageDetails { get; set; }
        public string StackTrace { get; set; }
        public DateTime? CreatedDateUtc { get; set; }
    }
}
