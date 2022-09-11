using WebApp.Logger.Loggers.Repositories;

namespace WebApp.Logger.Loggers
{
    public interface ILog
    {
        ISqlLogRepository Sql { get; }
        IExceptionLogRepository Error { get; }
        IAuditLogRepository Audit { get; }
        IRouteLogRepository Request { get; }
    }
}
