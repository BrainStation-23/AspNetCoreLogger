using WebApp.Logger.Loggers.Repositories;

namespace WebApp.Logger.Loggers
{
    public interface ILog
    {
        ISqlLogRepository Sql { get; }
        IErrorLogRepository Error { get; }
        IAuditLogRepository Audit { get; }
        IRequestLogRepository Request { get; }
    }
}
