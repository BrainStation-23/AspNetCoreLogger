using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApp.Logger.Providers.CosmosDbs;
using WebApp.Logger.Providers.Sqls;

namespace WebApp.Logger.Providers.Mongos
{
    public interface ICosmosDbRepository<TItem> where TItem : IItem
    {
        Task<TItem> DeleteAsync(string id);
        Task<TItem> GetAsync(string id);
        Task<dynamic> GetsAsync(string queryString, DapperPager pager);
        Task<TItem> InsertAsync(TItem item);
        Task<List<TItem>> InsertManyAsync(List<TItem> items);
        Task<TItem> UpdateAsync(string id, TItem item);

        /// <summary>
        /// delete logs before date
        /// </summary>
        /// <param name="dateTime">Date time </param>
        /// <param name="logType">Log type eg. sql, request, audit & error</param>
        /// <returns></returns>
        Task DeleteAsync(string dateTime, string logType);
    }

}
