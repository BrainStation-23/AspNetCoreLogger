using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Logger.Loggers;
using WebApp.Logger.Loggers.Repositories;

namespace WebApp.Logger.Hostings
{
    public class BatchLoggingBackGroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        Timer _loggingTimer;
        public BatchLoggingBackGroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _loggingTimer = new Timer(PublishToDb, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        public async void PublishToDb(object? state)
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var _exceptionLogRepository = scope.ServiceProvider.GetService<IExceptionLogRepository>();
                var _routeLogRepository = scope.ServiceProvider.GetService<IRouteLogRepository>();
                var _auditLogRepository = scope.ServiceProvider.GetService<IAuditLogRepository>();
                var _sqlLogRepository = scope.ServiceProvider.GetService<ISqlLogRepository>();
                BatchLoggingContext.PublishToDbAsync(
                    _routeLogRepository
                    , _sqlLogRepository
                    , _exceptionLogRepository
                    , _auditLogRepository);
            }
        }

    }
}
