using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Logger.Loggers.Providers.Mongos;
using WebApp.Logger.Loggers.Repositories;
using WebApp.Logger.Models;
using WebApp.Logger.Providers;

namespace WebApp.Logger.Loggers
{
    public static class BatchLoggingContext
    {
        private static ConcurrentQueue<AuditEntry> auditLogs = new ConcurrentQueue<AuditEntry>();
        private static ConcurrentQueue<ErrorModel> errorLogs = new ConcurrentQueue<ErrorModel>();
        private static ConcurrentQueue<RequestModel> requestLogs = new ConcurrentQueue<RequestModel>();
        private static ConcurrentQueue<SqlModel> sqlLogs = new ConcurrentQueue<SqlModel>();

        private readonly static int maxBatchSize = 100;

        public static async Task PublishAsync<T>(this T log, string logType) where T : class
        {
            if (logType == LogType.Error.ToString())
                errorLogs.Enqueue(log as ErrorModel);

            else if (logType == LogType.Audit.ToString())
                auditLogs.Enqueue(log as AuditEntry);

            else if (logType == LogType.Request.ToString())
                requestLogs.Enqueue(log as RequestModel);

            else if (logType == LogType.Sql.ToString())
                sqlLogs.Enqueue(log as SqlModel);
        }
        public static async Task PublishAsync<T>(this List<T> logs, string logType) where T : class
        {
            if (logType == LogType.Error.ToString())
                logs.ForEach(log => { errorLogs.Enqueue(log as ErrorModel); });

            else if (logType == LogType.Audit.ToString())
                logs.ForEach(log => { auditLogs.Enqueue(log as AuditEntry); });

            else if (logType == LogType.Request.ToString())
                logs.ForEach(log => { requestLogs.Enqueue(log as RequestModel); });

            else if (logType == LogType.Sql.ToString())
                logs.ForEach(log => { sqlLogs.Enqueue(log as SqlModel); });
        }

        public static async void PublishToDbAsync(IRouteLogRepository routeLogRepository
            , ISqlLogRepository sqlLogRepository
            , IExceptionLogRepository exceptionLogRepository
            , IAuditLogRepository auditLogRepository)
        {
            List<AuditEntry> auditLogList = new List<AuditEntry>();
            List<SqlModel> sqlLogList = new List<SqlModel>();
            List<ErrorModel> errorLogList = new List<ErrorModel>();
            List<RequestModel> requestLogList = new List<RequestModel>();


            if (sqlLogs.Count >= maxBatchSize)
            {
                while (sqlLogs.TryDequeue(out SqlModel frontObj))
                {
                    sqlLogList.Add(frontObj);
                    if (sqlLogList.Count >= maxBatchSize)
                        break;
                }
            }

            if (auditLogs.Count >= maxBatchSize)
            {
                while (auditLogs.TryDequeue(out AuditEntry topObj))
                {
                    auditLogList.Add(topObj);
                    if (auditLogList.Count >= maxBatchSize)
                        break;
                }
            }

            if (requestLogs.Count >= maxBatchSize)
            {
                while (requestLogs.TryDequeue(out RequestModel topObj))
                {
                    requestLogList.Add(topObj);
                    if (requestLogList.Count >= maxBatchSize)
                        break;
                }
            }

            if (errorLogs.Count >= maxBatchSize)
            {
                while (errorLogs.TryDequeue(out ErrorModel topObj))
                {
                    errorLogList.Add(topObj);
                    if (errorLogList.Count >= maxBatchSize)
                        break;
                }
            }

            if (sqlLogList.Any())
                await sqlLogRepository.AddAsync(sqlLogList);

            if (auditLogList.Any())
                await auditLogRepository.AddAsync(auditLogList);

            if (requestLogList.Any())
                await routeLogRepository.AddAsync(requestLogList);

            if (errorLogList.Any())
                await exceptionLogRepository.AddAsync(errorLogList);
        }
    }
}
