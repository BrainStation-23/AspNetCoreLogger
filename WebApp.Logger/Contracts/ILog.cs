namespace WebApp.Logger.Contracts
{
    public interface ILog
    {
        ISqlLogRepository Sql { get; }
        IErrorLogRepository Error { get; }
        IAuditLogRepository Audit { get; }
        IRequestLogRepository Request { get; }
    }
}
