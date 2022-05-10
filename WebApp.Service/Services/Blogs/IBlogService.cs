using System.Threading.Tasks;
using WebApp.Core;
using WebApp.Core.Collections;
using WebApp.Service.Models.Blogs;
using WebApp.Services;
using WebApp.Sql.Entities.Blogs;

namespace WebApp.Service
{
    public interface IBlogService : IBaseService<BlogEntity>
    {
        Task<Paging<BlogModel>> GetSearchAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize, string searchText = null);
    }
}
