using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Logger.Loggers;
using WebApp.Logger.Loggers.Repositories;

namespace WebApp.Logger.Hostings
{
    public class BatchLoggingBackGroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        public Timer _loggingTimer;

        public BatchLoggingBackGroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _loggingTimer = new Timer(PublishToDb, null, TimeSpan.Zero, TimeSpan.FromSeconds(5000));
        }

        public async void PublishToDb(object? state)
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var _exceptionLogRepository = scope.ServiceProvider.GetService<IExceptionLogRepository>();
                var _routeLogRepository = scope.ServiceProvider.GetService<IRouteLogRepository>();
                var _auditLogRepository = scope.ServiceProvider.GetService<IAuditLogRepository>();
                var _sqlLogRepository = scope.ServiceProvider.GetService<ISqlLogRepository>();

                await BatchLoggingContext.BatchLogProceassAsync(
                    _routeLogRepository
                    , _sqlLogRepository
                    , _exceptionLogRepository
                    , _auditLogRepository
                    //,this
                    );
            }
        }

        public void ChangeTimerInterval()
        {
            // if cpu usage high, then publish low
            // if cpu usage low, then publish high
            _loggingTimer.Change(10000,10000);
        }

    }
}
