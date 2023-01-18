using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Logger.Hostings;
using WebApp.Logger.Loggers.Providers.Mongos;
using WebApp.Logger.Loggers.Repositories;
using WebApp.Logger.Models;
using WebApp.Logger.Providers;

namespace WebApp.Logger.Loggers
{
    public static class BatchLoggingContext
    {
        private static ConcurrentQueue<AuditEntry> auditLogQueue = new ConcurrentQueue<AuditEntry>();
        private static ConcurrentQueue<ErrorModel> errorLogQueue = new ConcurrentQueue<ErrorModel>();
        private static ConcurrentQueue<RequestModel> requestLogQueue = new ConcurrentQueue<RequestModel>();
        private static ConcurrentQueue<SqlModel> sqlLogQueue = new ConcurrentQueue<SqlModel>();

        private readonly static int maxBatchSize = 100;

        public static async Task PublishAsync<T>(this T log, string logType) where T : class
        {
            if (logType == LogType.Error.ToString())
                errorLogQueue.Enqueue(log as ErrorModel);
            else if (logType == LogType.Audit.ToString())
                auditLogQueue.Enqueue(log as AuditEntry);
            else if (logType == LogType.Request.ToString())
                requestLogQueue.Enqueue(log as RequestModel);
            else if (logType == LogType.Sql.ToString())
                sqlLogQueue.Enqueue(log as SqlModel);
        }

        public static async Task PublishAsync<T>(this List<T> logs, string logType) where T : class
        {
            logs.ForEach(async log => await PublishAsync(log, logType));
        }

        public static async Task BatchLogProcessAsync(IRouteLogRepository routeLogRepository
            , ISqlLogRepository sqlLogRepository
            , IExceptionLogRepository exceptionLogRepository
            , IAuditLogRepository auditLogRepository)
        {
            List<AuditEntry> auditLogs = auditLogQueue.GetLogList(maxBatchSize);
            List<SqlModel> sqlLogs = sqlLogQueue.GetLogList(maxBatchSize);
            List<ErrorModel> errorLogs = errorLogQueue.GetLogList(maxBatchSize);
            List<RequestModel> requestLogs = requestLogQueue.GetLogList(maxBatchSize);

            if (sqlLogs.Any())
                await sqlLogRepository.AddAsync(sqlLogs);

            if (auditLogs.Any())
                await auditLogRepository.AddAsync(auditLogs);

            if (requestLogs.Any())
                await routeLogRepository.AddAsync(requestLogs);

            if (errorLogs.Any())
                await exceptionLogRepository.AddAsync(errorLogs);

        }

        public static List<T> GetLogList<T>(this ConcurrentQueue<T> logQueue, int maxListSize)
        {
            List<T> logs = new List<T>();

            if (!logQueue.IsEmpty)
            {
                while (logQueue.TryDequeue(out T log))
                {
                    logs.Add(log);
                    if (logs.Count >= maxListSize)
                        break;
                }
            }

            return logs;
        }
    }
}
