using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Common.Serialize;
using WebApp.Logger.DashboardModels;
using WebApp.Logger.Loggers.Providers.Sqls;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Loggers.Providers
{
    public interface IDashboardRepository
    {

        Task<dynamic> GetAuditPageByDateAsync(DateTime startDateTime, DateTime endDateTime, DapperPager pager);

        Task<dynamic> GetAuditPageByDateAsync(string TraceId);

        Task<dynamic> GetRequestPageByDateAsync(DateTime startDateTime, DateTime endDateTime, DapperPager pager);

        Task<dynamic> GetRequestPageByDateAsync(string TraceId);

        Task<dynamic> GetSqlPageByDateAsync(DateTime startDateTime, DateTime endDateTime, DapperPager pager);

        Task<dynamic> GetSqlPageByDateAsync(string TraceId);

        Task<dynamic> GetErrorPageByDateAsync(DateTime startDateTime, DateTime endDateTime, DapperPager pager);

        Task<dynamic> GetErrorPageByDateAsync(string TraceId);

        Task<dynamic> GetTopRequestsAsync();

        Task<dynamic> GetTopExceptionAsync();

        Task<dynamic> GetLogCountSummaryAsync();

        Task<dynamic> GetSlowestRequestAsync();

        Task<dynamic> GetFastestRequestAsync();
    }
}
