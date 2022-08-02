using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Core;
using WebApp.Common.Collections;

namespace WebApp.Services
{
    public interface IBaseService<TEntity, TModel> where TEntity : class where TModel: class
    {
        Task<Paging<TModel>> GetPageAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize);
        Task<List<TModel>> GetAllAsync();
        Task<TModel> FindAsync(long Id);
        Task<TModel> FirstOrDefaultAsync(long id);
        Task<TModel> InsertAsync(TModel model);
        Task<List<TModel>> InsertRangeAsync(List<TModel> models);
        Task<TModel> UpdateAsync(long id, TModel model);
        Task<TModel> UpdateAsync(TModel model);
        Task<List<TModel>> UpdateRangeAsync(List<TModel> models);
        Task<TModel> DeleteAsync(long id);
        Task<TModel> DeleteAsync(TModel model);
        Task<List<TModel>> DeleteRangeAsync(List<TModel> models);
    }
}
