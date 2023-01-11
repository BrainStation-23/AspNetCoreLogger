using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using System.Data.Common;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Common.Extensions;
using WebApp.Common.Serialize;
using WebApp.Logger.Loggers;
using WebApp.Logger.Loggers.Repositories;
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
                Source = "Query",
                ApplicationName = "",
                UserId = context.User.Identity?.IsAuthenticated ?? false ? long.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier)) : null,
                IpAddress = context.GetIpAddress(),
                Host = context.Request.Host.ToString(),
                Url = context.Request.GetDisplayUrl() ?? context.Request.GetEncodedUrl(),
                TraceId = context.TraceIdentifier,
                Scheme = context.Request.Scheme,
                Protocol = context.Request.Protocol,
                Version = "",
                UrlReferrer = "",
                Area = "",
                ControllerName = context.Request.RouteValues["controller"].ToString(),
                ActionName = context.Request.RouteValues["action"].ToString(),
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

            await model.AddToLogBatch(LogType.Sql.ToString());
        }
    }
}

// https://docs.microsoft.com/en-us/ef/core/logging-events-diagnostics/simple-logging
// https://docs.microsoft.com/en-us/ef/core/logging-events-diagnostics/interceptors
// https://davecallan.com/log-sql-queries-entity-framework-core-3-interceptors/
