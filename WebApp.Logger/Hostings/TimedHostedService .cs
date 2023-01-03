using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Logger.Loggers.Repositories;

namespace WebApp.Logger.Hostings
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<TimedHostedService> _logger;
        private Timer _timer = null;
        private readonly ISqlLogRepository _sqlLogRepository;
        public TimedHostedService(ISqlLogRepository sqlLogRepository)
        {
            _sqlLogRepository = sqlLogRepository;
        }
        public TimedHostedService(ILogger<TimedHostedService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(20));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var retentionDays = 0;
            var currentDate = DateTime.Now;
            var retentionDate = currentDate.AddDays(-retentionDays).Date;
            _sqlLogRepository.DeleteRetention(retentionDate);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}


