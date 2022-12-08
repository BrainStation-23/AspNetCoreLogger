﻿using System.Threading.Tasks;
using WebApp.Logger.Providers.Sqls;
using WebApp.Logger.Models;

namespace WebApp.Logger.Loggers.Repositories
{
    public interface IExceptionLogRepository
    {
        Task AddAsync(ErrorModel errorModel);
        Task<dynamic> GetPageAsync(DapperPager pager);
    }
}