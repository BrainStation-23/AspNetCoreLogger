namespace WebApp.Logger.DashboardModels
{
    public class SummaryDashboardModel
    {
        public long TotalAuditLogs { get; set; }
        public long TotalSqlLogs { get; set; }
        public long TotalErrorLogs { get; set; }
        public long TotalRequestLogs { get; set; }
    }
}
