using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Common.Serialize;
using WebApp.Logger.Providers.Sqls;
using WebApp.Logger.Models;

namespace WebApp.Logger.Loggers.Repositories
{
    public class ExceptionLogRepository : IExceptionLogRepository
    {
        private readonly DapperContext _dapper;
        private readonly ILogger<ExceptionLogRepository> _logger;

        public ExceptionLogRepository(DapperContext dapper,
            ILogger<ExceptionLogRepository> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }

        public async Task AddAsync(ErrorModel errorModel)
        {
            if (errorModel.Url.Contains("/Log/"))
                return;

            var createdDateUtc = DateTime.UtcNow.ToString();
            var query = @"INSERT INTO [dbo].[ExceptionLogs]
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
                               , @CreatedDateUtc)";

            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    await connection.ExecuteAsync(query, new
                    {
                        UserId = errorModel.UserId,
                        ApplicationName = errorModel.Application,
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
                        CreatedDateUtc = createdDateUtc
                    });
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(ExceptionLogRepository), exception);
            }
        }

        public async Task<dynamic> GetPageAsync(DapperPager pager)
        {
            dynamic logs;

            var query = @"SELECT * FROM [dbo].[ExceptionLogs]
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
                    exceptionLogUnescapeString = exceptionLogs.ToJson();
                    var unescape = exceptionLogUnescapeString.JsonUnescaping();
                    logs = JArray.Parse(unescape);
                }

                return logs;
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(ExceptionLogRepository), exception);
                throw;
            }
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
