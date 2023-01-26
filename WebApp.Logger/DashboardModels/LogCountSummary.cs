using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Logger.DashboardModels
{
    public class LogsCountSummary
    {
        public long TotalAuditLogs { get; set; }
        public long TotalSqlLogs { get; set; }
        public long TotalErrorLogs { get; set; }
        public long TotalRequestLogs { get; set; }
    }
}
