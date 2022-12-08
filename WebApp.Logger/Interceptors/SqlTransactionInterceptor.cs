﻿using Microsoft.AspNetCore.Http;
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
    public class SqlTransactionInterceptor : DbTransactionInterceptor
    {
        private readonly IHttpContextAccessor Context;
        private readonly ISqlLogRepository SqlLogRepository;
        private readonly LogOption _logOption;

        public SqlTransactionInterceptor(IHttpContextAccessor context,
            ISqlLogRepository sqlLogRepository,
            IOptions<LogOption> logOption)
        {
            Context = context;
            SqlLogRepository = sqlLogRepository;
            _logOption = logOption.Value;
        }

        public override async ValueTask<InterceptionResult> TransactionCommittingAsync(DbTransaction transaction,
            TransactionEventData eventData,
            InterceptionResult result,
            CancellationToken cancellationToken = default)
        {
            //await ManipulateCommandAsync(command, eventData);
            return result;
        }

        private async Task ManipulateCommandAsync(DbTransaction transaction, TransactionEventData eventData)
        {
            if (_logOption.LogType.Contains(LogType.Sql.ToString()))
                return;

            var context = Context.HttpContext;
            var model = new SqlModel
            {
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
                Event = new
                {
                    eventData.EventId.Id,
                    eventData.EventId.Name,
                }.ToJson()
            };

            await SqlLogRepository.AddAsync(model);
        }
    }
}