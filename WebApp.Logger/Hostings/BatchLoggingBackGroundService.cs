using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Logger.Contracts;
using WebApp.Logger.Loggers;

namespace WebApp.Logger.Hostings
{
    public class BatchLoggingBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public Timer _loggingTimer;

        public BatchLoggingBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _loggingTimer = new Timer(ProcessBatchLog, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));

            return Task.CompletedTask;
        }

        public async void ProcessBatchLog(object state)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();
            var _exceptionLogRepository = scope.ServiceProvider.GetService<IErrorLogRepository>();
            var _routeLogRepository = scope.ServiceProvider.GetService<IRequestLogRepository>();
            var _auditLogRepository = scope.ServiceProvider.GetService<IAuditLogRepository>();
            var _sqlLogRepository = scope.ServiceProvider.GetService<ISqlLogRepository>();
            var _traceRepository = scope.ServiceProvider.GetService<ITraceRepository>();

            await BatchLoggingContext.BatchLogProcessAsync(_routeLogRepository
                , _sqlLogRepository
                , _exceptionLogRepository
                , _auditLogRepository
                , _traceRepository);
        }

        public void ChangeTimerInterval()
        {
            // if cpu usage high, then publish low
            // if cpu usage low, then publish high
            _loggingTimer.Change(10000, 10000);
        }

    }
}
