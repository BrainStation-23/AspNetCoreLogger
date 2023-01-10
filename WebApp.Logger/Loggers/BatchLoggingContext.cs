using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Logger.Loggers.Repositories;
using WebApp.Logger.Models;

namespace WebApp.Logger.Loggers
{
    public static class BatchLoggingContext
    {
        public static Queue<AuditEntry> auditLogs = new Queue<AuditEntry>();
        public static Queue<ErrorModel> errorLogs = new Queue<ErrorModel>();
        public static Queue<RequestModel> requestLogs = new Queue<RequestModel>();
        public static Queue<SqlModel> sqlLogs = new Queue<SqlModel>();

        public static void SaveToQueue(this AuditEntry log)
        {
            auditLogs.Enqueue(log);
        }

        public static void SaveToQueue(this ErrorModel log)
        {
            errorLogs.Enqueue(log);
        }

        public static void SaveToQueue(this RequestModel log)
        {
            requestLogs.Enqueue(log);
        }

        public static void SaveToQueue(this SqlModel log)
        {
            sqlLogs.Enqueue(log);
        }

        public static void SaveToQueue(this List<AuditEntry> logs)
        {

            logs.ForEach(log =>
            {
                auditLogs.Enqueue(log);
            });
        }

        public static void SaveToQueue(this List<ErrorModel> logs)
        {
            logs.ForEach(log =>
            {
                errorLogs.Enqueue(log);
            });
        }

        public static void SaveToQueue(this List<RequestModel> logs)
        {
            logs.ForEach(log =>
            {
                requestLogs.Enqueue(log);
            });
        }

        public static void SaveToQueue(this List<SqlModel> logs)
        {
            logs.ForEach(log =>
            {
                sqlLogs.Enqueue(log);
            });
        }

        public static void SaveAllLogsToDatabase(IRouteLogRepository routeLogRepository
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
            sqlLogRepository.AddAsync(sqlLogList);

            while (auditLogs.Count > 0)
            {
                auditLogList.Add(auditLogs.Dequeue());
            }
            auditLogRepository.AddAsync(auditLogList);

            while (requestLogs.Count > 0)
            {
                requestLogList.Add(requestLogs.Dequeue());
            }
            routeLogRepository.AddAsync(requestLogList);

            while (errorLogs.Count > 0)
            {
                errorLogList.Add(errorLogs.Dequeue());
            }
            exceptionLogRepository.AddAsync(errorLogList);
        }
    }
}
