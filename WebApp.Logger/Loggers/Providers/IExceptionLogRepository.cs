using System.Threading.Tasks;
using WebApp.Logger.Providers.Sqls;
using WebApp.Logger.Models;
using System;
using System.Collections.Generic;

namespace WebApp.Logger.Loggers.Repositories
{
    public interface IExceptionLogRepository
    {
        Task AddAsync(ErrorModel errorModel);
        Task AddAsync(List<ErrorModel> errorModel);
        Task<dynamic> GetPageAsync(DapperPager pager);
        Task RetentionAsync(DateTime dateTime);
    }
}