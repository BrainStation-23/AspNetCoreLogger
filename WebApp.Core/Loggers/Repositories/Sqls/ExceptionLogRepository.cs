using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using WebApp.Common.Serialize;
using WebApp.Core.Contexts;
using WebApp.Core.Models;

namespace WebApp.Core.Loggers.Repositories
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
                        Errors = JsonSerializer.Serialize(errorModel.Errors),
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
            dynamic exceptionLogs;
            var query = @"SELECT * FROM [dbo].[ExceptionLogs]
                            ORDER BY [Id] DESC
                            OFFSET @Offset ROWS 
                            FETCH NEXT @Next ROWS ONLY";

            try
            {
                using (var connection = _dapper.CreateConnection())
                {
                    var exceptionLogEntities = await connection.QueryAsync(query, pager);
                    var exceptionLogUnescapeString = exceptionLogEntities.ToJson().JsonUnescaping();
                    exceptionLogs = JArray.Parse(exceptionLogUnescapeString);
                }

                return exceptionLogs;
            }
            catch (Exception exception)
            {
                _logger.LogError(nameof(ExceptionLogRepository), exception);
                throw;
            }
        }
    }
}
