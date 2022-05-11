using WebApp.Core.Sqls;
using WebApp.Entity.Entities.Logs;

namespace WebApp.Sql.Repositories
{
    public class RequestLogRepository : SqlRepository<RequestLog>, IRequestLogRepository
    {
        public RequestLogRepository(WebAppContext dbContext) : base(dbContext)
        {
        }
    }
}
