using Microsoft.Extensions.DependencyInjection;
using System;
using WebApp.Logger.Loggers.Repositories;

namespace WebApp.Logger.Loggers.Providers.Sqls
{
    public class SqlProvider : ILog
    {
        private readonly IServiceProvider _serviceProvider;

        public SqlProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ISqlLogRepository Sql => ActivatorUtilities.CreateInstance<SqlLogRepository>(_serviceProvider);
        public IErrorLogRepository Error => ActivatorUtilities.CreateInstance<ErrorLogRepository>(_serviceProvider);
        public IAuditLogRepository Audit => ActivatorUtilities.CreateInstance<AuditLogRepository>(_serviceProvider);
        public IRequestLogRepository Request => ActivatorUtilities.CreateInstance<RequestLogRepository>(_serviceProvider);
    }
}
