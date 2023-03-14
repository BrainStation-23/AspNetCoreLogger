using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Logger.Extensions;
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
                Source = SqlSource.Transaction.ToString(),
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
                Duration = (DateTimeOffset.Now - eventData.StartTime).TotalMilliseconds,
                Event = new
                {
                    eventData.EventId.Id,
                    eventData.EventId.Name,
                }
            };

            // await SqlLogRepository.AddAsync(model);

            await BatchLoggingContext.PublishAsync(model, LogType.Sql.ToString());
        }
    }
}