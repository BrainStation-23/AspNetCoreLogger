using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebApp.Logger.Loggers;
using WebApp.Logger.Loggers.Repositories;

namespace WebApp.Logger.Hostings
{
    public class DefaultScopedProcessingService : IScopedProcessingService
    {
        private int _executionCount;
        private readonly ILogger<DefaultScopedProcessingService> _logger;
        private readonly IExceptionLogRepository _exceptionLogRepository;
        private readonly IRouteLogRepository _routeLogRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly ISqlLogRepository _sqlLogRepository;

        private readonly LogOption _logOptions;
        //public DefaultScopedProcessingService(
        //    ILogger<DefaultScopedProcessingService> logger) =>
        //    _logger = logger;

        public DefaultScopedProcessingService(IExceptionLogRepository exceptionLogRepository,
            IRouteLogRepository routeLogRepository,
            IAuditLogRepository auditLogRepository,
            ISqlLogRepository sqlLogRepository,
            ILogger<DefaultScopedProcessingService> logger,
            IOptions<LogOption> logOptions)
        {
            _exceptionLogRepository = exceptionLogRepository;
            _routeLogRepository = routeLogRepository;
            _auditLogRepository = auditLogRepository;
            _sqlLogRepository = sqlLogRepository;
            _logger = logger;
            _logOptions = logOptions.Value;
        }

        public async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                ++_executionCount;

                _logger.LogInformation(
                    "{ServiceName} working, execution count: {Count}",
                    nameof(DefaultScopedProcessingService),
                    _executionCount);

                var retentionDate = GetRetentionTime();

                if (retentionDate != null)
                {
                    DateTime date = (DateTime)retentionDate;
                    await _exceptionLogRepository.RetentionAsync(date);
                    await _routeLogRepository.RetentionAsync(date);
                    await _auditLogRepository.RetentionAsync(date);
                    await _sqlLogRepository.RetentionAsync(date);
                }
                await Task.Delay(10000, stoppingToken);
            }
        }

        public DateTime? GetRetentionTime()
        {
            string retentionDays = null;

            if (_logOptions.ProviderType == ProviderType.MSSql.ToString())
                retentionDays = _logOptions.Provider.MSSql.Retention;

            else if (_logOptions.ProviderType == ProviderType.File.ToString())
                retentionDays = _logOptions.Provider.File.Retention;

            else if (_logOptions.ProviderType == ProviderType.CosmosDb.ToString())
                retentionDays = _logOptions.Provider.CosmosDb.Retention;

            else
                retentionDays = _logOptions.Provider.Mongo.Retention;

            if (retentionDays is null && _logOptions.Retention is not null)
                retentionDays = _logOptions.Retention.ToString();

            if (retentionDays is null)
                return null;
            else
                retentionDays=retentionDays.ToLower();

            char ch = retentionDays[retentionDays.Length- 1];
            int mul = 1;

            if (ch == 'm')
                mul = 30;
            else if (ch == 'y')
                mul = 365;

            retentionDays = retentionDays.Remove(retentionDays.Length - 1);
            var currentDate = DateTime.UtcNow;

            return currentDate.AddDays(-Int32.Parse(retentionDays)*mul).Date;

        }
    }
}
