using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Contracts
{
    public interface ITraceRepository
    {
        Task AddAsync(TraceModel model);
        Task AddAsync(List<TraceModel> model);
        Task<dynamic> GetPageAsync(DapperPager pager);
        Task RetentionAsync(DateTime dateTime);
    }
}