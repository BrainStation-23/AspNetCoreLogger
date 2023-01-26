using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Loggers.Providers.Files
{
    public class FileDashboardRepository//:IDashboardRepository
    {
        public Task<dynamic> GetAuditPageByDateAsync(DateTime startDateTime, DateTime endDateTime, DapperPager pager)
        {
            return null;
        }

        public Task<dynamic> GetRequestPageByDateAsync(DateTime startDateTime, DateTime endDateTime, DapperPager pager)
        {
            return null;
        }
        public Task<dynamic> GetSqlPageByDateAsync(DateTime startDateTime, DateTime endDateTime, DapperPager pager)
        {
            return null;
        }
        public Task<dynamic> GetErrorPageByDateAsync(DateTime startDateTime, DateTime endDateTime, DapperPager pager)
        {
            return null;
        }
        public Task<dynamic> GetSummaryAsync()
        {
            return null;
        }
    }
}
