using Microsoft.Extensions.DependencyInjection;
using System;
using WebApp.Logger.Loggers.Repositories;

namespace WebApp.Logger.Loggers.Providers.Sqls
{
    public class FileProvider : ILog
    {
        private readonly IServiceProvider _serviceProvider;

        public FileProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ISqlLogRepository Sql => ActivatorUtilities.CreateInstance<SqlFileLogRepository>(_serviceProvider);
        public IExceptionLogRepository Error => ActivatorUtilities.CreateInstance<ExceptionFileLogRepository>(_serviceProvider);
        public IAuditLogRepository Audit => ActivatorUtilities.CreateInstance<AuditLogRepository>(_serviceProvider);
        public IRouteLogRepository Request => ActivatorUtilities.CreateInstance<RouteFileLogRepository>(_serviceProvider);
    }
}
