using System.Collections.Generic;
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
    }

}
