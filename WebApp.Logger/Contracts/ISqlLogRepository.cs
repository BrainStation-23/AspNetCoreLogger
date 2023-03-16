using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Logger.Models;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Contracts
{
    public interface ISqlLogRepository
    {
        Task AddAsync(SqlModel sqlModel);
        Task AddAsync(List<SqlModel> sqlModel);
        Task<dynamic> GetPageAsync(DapperPager pager);
        Task RetentionAsync(DateTime dateTime);
    }
}