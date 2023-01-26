using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using System;
using System.Data.Common;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Common.DataType;
using WebApp.Common.Extensions;
using WebApp.Common.Serialize;
using WebApp.Logger.Loggers;
using WebApp.Logger.Loggers.Repositories;
using WebApp.Logger.Models;

namespace WebApp.Logger.Interceptors
{
    public class SqlTransactionInterceptor : DbTransactionInterceptor
    {
        private readonly IHttpContextAccessor Context;
        private readonly ISqlLogRepository SqlLogRepository;

        public SqlTransactionInterceptor(IHttpContextAccessor context,
            ISqlLogRepository sqlLogRepository)
        {
            Context = context;
            SqlLogRepository = sqlLogRepository;
        }

        public override async ValueTask<InterceptionResult> TransactionCommittingAsync(DbTransaction transaction,
            TransactionEventData eventData,
            InterceptionResult result,
            CancellationToken cancellationToken = default)
        {
            await ManipulateCommandAsync(transaction, eventData);
            return result;
        }

        private async Task ManipulateCommandAsync(DbTransaction transaction, TransactionEventData eventData)
        {

            var context = Context.HttpContext;
            var model = new SqlModel
            {
                Source = "Transaction",
                ApplicationName = AppDomain.CurrentDomain.FriendlyName.ToString(),
                UserId = context.User.Identity?.IsAuthenticated ?? false ? long.Parse(context.User.FindFirstValue(ClaimTypes.NameIdentifier)) : null,
                IpAddress = context.GetIpAddress(),
                Host = context.Request.Host.ToString(),
                Url = context.Request.GetDisplayUrl() ?? context.Request.GetEncodedUrl(),
                TraceId = context.TraceIdentifier,
                Scheme = context.Request.Scheme,
                Protocol = context.Request.Protocol,
                Version = (string)context.Features.GetPropValue("HttpVersion"),
                UrlReferrer = context.Request.Headers["Referer"].ToString(),
                Area = "",
                ControllerName = context.Request.RouteValues["controller"].ToString(),
                ActionName = context.Request.RouteValues["action"].ToString(),
                ClassName = "",
                MethodName = "",
                Duration = (DateTimeOffset.Now - eventData.StartTime).TotalMilliseconds,
                Event = new
                {
                    eventData.EventId.Id,
                    eventData.EventId.Name,
                }
            };

            // await SqlLogRepository.AddAsync(model);

            await BatchLoggingContext.PublishAsync(model,LogType.Sql.ToString());
        }
    }
}