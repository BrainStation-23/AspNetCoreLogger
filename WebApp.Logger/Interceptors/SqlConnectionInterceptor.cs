using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using System;
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
    public class SqlConnectionInterceptor : DbConnectionInterceptor
    {
        private readonly IHttpContextAccessor Context;
        private readonly ISqlLogRepository SqlLogRepository;
        private readonly LogOption _logOption;

        public SqlConnectionInterceptor(IHttpContextAccessor context,
            ISqlLogRepository sqlLogRepository)
        {
            Context = context;
            SqlLogRepository = sqlLogRepository;
        }

        //public override InterceptionResult ConnectionOpening(DbConnection connection,
        //    ConnectionEventData eventData,
        //    InterceptionResult result)
        //    => throw new InvalidOperationException("Open connections asynchronously when using AAD authentication.");

        public override async ValueTask<InterceptionResult> ConnectionOpeningAsync(DbConnection connection,
            ConnectionEventData eventData,
            InterceptionResult result,
            CancellationToken cancellationToken = default)
        {
            var sqlConnection = (SqlConnection)connection;

            await ManipulateCommandAsync(sqlConnection, eventData);

            return result;
        }

        private async Task ManipulateCommandAsync(SqlConnection connection, ConnectionEventData commandExecutedEventData)
        {
            var context = Context.HttpContext;
            var model = new SqlModel
            {
                Source = "Connection",
                ApplicationName = "",
                UserId = context.User.Identity?.IsAuthenticated ?? false ? long.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier)) : null,
                IpAddress = context.GetIpAddress(),
                Host = context.Request.Host.ToString(),
                Url = context.Request.GetDisplayUrl() ?? context.Request.GetEncodedUrl(),
                TraceId = context.TraceIdentifier,
                Scheme = context.Request.Scheme,
                Proctocol = context.Request.Protocol,
                Version = "",
                UrlReferrer = "",
                Area = "",
                ControllerName = "",
                ActionName = "",
                ClassName = "",
                MethodName = "",
                Query = "",
                QueryType = "",
                Duration = 0,
                //Response = commandExecutedEventData.Result.ToJson(),
                Connection = new
                {
                    commandExecutedEventData.Connection.Database,
                    commandExecutedEventData.Connection.DataSource,
                    commandExecutedEventData.ConnectionId,
                    ConnectionTimeout = ((SqlConnection)commandExecutedEventData.Connection).ConnectionTimeout
                }.ToJson(),
                Command = new
                {
                    CommandTimeout = 0,
                    CommandType = ""
                }.ToJson(),
                Event = new
                {
                    commandExecutedEventData.EventId.Id,
                    commandExecutedEventData.EventId.Name,
                }.ToJson(),
            };
            await SqlLogRepository.AddAsync(model);
        }
    }
}

// https://docs.microsoft.com/en-us/ef/core/logging-events-diagnostics/simple-logging
// https://docs.microsoft.com/en-us/ef/core/logging-events-diagnostics/interceptors
// https://davecallan.com/log-sql-queries-entity-framework-core-3-interceptors/
