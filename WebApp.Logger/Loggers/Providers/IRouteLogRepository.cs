using System.Threading.Tasks;
using WebApp.Logger.Providers.Sqls;
using WebApp.Logger.Models;
using System;
using System.Collections.Generic;

namespace WebApp.Logger.Loggers.Repositories
{
    public interface IRouteLogRepository
    {
        Task AddAsync(RequestModel requestModel);
        Task AddAsync(List<RequestModel> requestModel);
        Task<dynamic> GetPageAsync(DapperPager pager);
        Task RetentionAsync(DateTime dateTime);
    }
}