using System;
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
        public static Queue<AuditEntry> auditLogs = new Queue<AuditEntry>();
        public static Queue<ErrorModel> errorLogs = new Queue<ErrorModel>();
        public static Queue<RequestModel> requestLogs = new Queue<RequestModel>();
        public static Queue<SqlModel> sqlLogs = new Queue<SqlModel>();

        public static async Task AddToLogBatch<T>(this T log,string logType) where T: class
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
        public static async Task AddToLogBatch<T>(this List<T> logs,string logType) where T : class
        {
            if(logType==LogType.Error.ToString())
                logs.ForEach(log =>{errorLogs.Enqueue(log as ErrorModel);});

            else if (logType == LogType.Audit.ToString())
                logs.ForEach(log => { auditLogs.Enqueue(log as AuditEntry); });

            else if(logType == LogType.Request.ToString())
                logs.ForEach(log => { requestLogs.Enqueue(log as RequestModel); });

            else if(logType == LogType.Sql.ToString())
                logs.ForEach(log => {sqlLogs.Enqueue(log as SqlModel); });
        }

        public static async Task SaveAllLogsToDatabase(IRouteLogRepository routeLogRepository
            ,ISqlLogRepository sqlLogRepository
            ,IExceptionLogRepository exceptionLogRepository
            ,IAuditLogRepository auditLogRepository)
        {
            List<AuditEntry> auditLogList = new List<AuditEntry>();
            List<SqlModel> sqlLogList = new List<SqlModel>();
            List<ErrorModel> errorLogList = new List<ErrorModel>();
            List<RequestModel> requestLogList = new List<RequestModel>();


            while (sqlLogs.Count > 0)
            {
                sqlLogList.Add(sqlLogs.Dequeue());
            }

            while (auditLogs.Count > 0)
            {
                auditLogList.Add(auditLogs.Dequeue());
            }

            while (requestLogs.Count > 0)
            {
                requestLogList.Add(requestLogs.Dequeue());
            }

            while (errorLogs.Count > 0)
            {
                errorLogList.Add(errorLogs.Dequeue());
            }

            await sqlLogRepository.AddAsync(sqlLogList);
            await auditLogRepository.AddAsync(auditLogList);
            await routeLogRepository.AddAsync(requestLogList);
            await exceptionLogRepository.AddAsync(errorLogList);
        }
    }
}
