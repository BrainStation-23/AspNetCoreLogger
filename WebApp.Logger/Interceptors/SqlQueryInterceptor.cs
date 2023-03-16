using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Logger.Contracts;
using WebApp.Logger.Extensions;
using WebApp.Logger.Loggers;
using WebApp.Logger.Models;

namespace WebApp.Logger.Interceptors
{
    public class SqlQueryInterceptor : DbCommandInterceptor
    {
        private readonly IHttpContextAccessor Context;
        private readonly ISqlLogRepository SqlLogRepository;

        public SqlQueryInterceptor(IHttpContextAccessor context,
            ISqlLogRepository sqlLogRepository)
        {
            Context = context;
            SqlLogRepository = sqlLogRepository;
        }

        public override DbDataReader ReaderExecuted(DbCommand command,
           CommandExecutedEventData eventData,
           DbDataReader result)
        {
            Task.Run(async () => await ManipulateCommandAsync(command, eventData));

            return result;
        }

        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = default)
        {
            return new ValueTask<InterceptionResult<DbDataReader>>(result);
        }

        public override async ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command,
            CommandExecutedEventData eventData,
            DbDataReader result,
            CancellationToken cancellationToken = default)
        {
            await ManipulateCommandAsync(command, eventData);

            return result;
        }

        //public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command,
        //    CommandEventData eventData,
        //    InterceptionResult<DbDataReader> result)
        //{
        //    ManipulateCommand(command);

        //    return result;
        //}

        //public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command,
        //    CommandEventData eventData,
        //    InterceptionResult<DbDataReader> result,
        //    CancellationToken cancellationToken = default)
        //{
        //    ManipulateCommand(command, eventData);

        //    return new ValueTask<InterceptionResult<DbDataReader>>(result);
        //}

        private async Task ManipulateCommandAsync(DbCommand command, CommandExecutedEventData commandExecutedEventData)
        {
            //if (_logOption.LogType.Contains(LogType.Sql.ToString()))
            //    return;

            var context = Context.HttpContext;
            var model = new SqlModel
            {
                Source = SqlSource.Query.ToString(),
                ApplicationName = AppDomain.CurrentDomain.FriendlyName.ToString(),
                UserId = context.GetUserId(),
                IpAddress = context.GetIpAddress(),
                Host = context.Request.Host.ToString(),
                Url = context.GetUrl(),
                TraceId = context.TraceIdentifier,
                Scheme = context.Request.Scheme,
                Protocol = context.Request.Protocol,
                Version = context.GetHttpVersion(),
                UrlReferrer = context.GetUrlReferrer(),
                Area = "",
                ControllerName = context.GetControllerName(),
                ActionName = context.GetActionName(),
                ClassName = "",
                MethodName = "",
                Query = command.CommandText,
                QueryType = command.CommandType.ToString(),
                Duration = commandExecutedEventData.Duration.TotalMilliseconds,
                //Response = commandExecutedEventData.Result.ToJson(),
                Connection = new
                {
                    commandExecutedEventData.Connection.Database,
                    commandExecutedEventData.Connection.DataSource,
                    commandExecutedEventData.Connection.ServerVersion,
                    commandExecutedEventData.ConnectionId,
                    ConnectionTimeout = ((Microsoft.Data.SqlClient.SqlConnection)commandExecutedEventData.Connection).ConnectionTimeout

                },
                Command = new
                {
                    CommandTimeout = ((Microsoft.Data.SqlClient.SqlCommand)commandExecutedEventData.Command).CommandTimeout,
                    CommandType = command.CommandType.ToString(),
                },
                Event = new
                {
                    commandExecutedEventData.EventId.Id,
                    commandExecutedEventData.EventId.Name,
                }
            };

            //await SqlLogRepository.AddAsync(model);

            await BatchLoggingContext.PublishAsync(model, LogType.Sql.ToString());
        }
    }
}

// https://docs.microsoft.com/en-us/ef/core/logging-events-diagnostics/simple-logging
// https://docs.microsoft.com/en-us/ef/core/logging-events-diagnostics/interceptors
// https://davecallan.com/log-sql-queries-entity-framework-core-3-interceptors/
