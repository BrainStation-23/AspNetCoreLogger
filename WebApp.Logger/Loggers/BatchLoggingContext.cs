using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        public static async Task PublishAsyncAsync<T>(this List<T> logs, string logType) where T : class
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

        public static async Task SaveAllLogsToDatabase(IRouteLogRepository routeLogRepository
            , ISqlLogRepository sqlLogRepository
            , IExceptionLogRepository exceptionLogRepository
            , IAuditLogRepository auditLogRepository)
        {
            List<AuditEntry> auditLogList = new List<AuditEntry>();
            List<SqlModel> sqlLogList = new List<SqlModel>();
            List<ErrorModel> errorLogList = new List<ErrorModel>();
            List<RequestModel> requestLogList = new List<RequestModel>();

            while (sqlLogs.IsEmpty is false)
            {
                sqlLogs.TryDequeue(out SqlModel frontObj);
                sqlLogList.Add(frontObj);
            }

            while (auditLogs.IsEmpty is false)
            {
                auditLogs.TryDequeue(out AuditEntry topObj);
                auditLogList.Add(topObj);
            }

            while (requestLogs.IsEmpty is false)
            {
                requestLogs.TryDequeue(out RequestModel topObj);
                requestLogList.Add(topObj);
            }

            while (errorLogs.IsEmpty is false)
            {
                errorLogs.TryDequeue(out ErrorModel topObj);
                errorLogList.Add(topObj);
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
