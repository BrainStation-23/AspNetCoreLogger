using System.Threading.Tasks;
using WebApp.Core;
using WebApp.Core.Collections;
using WebApp.Service.Models.Blogs;
using WebApp.Services;
using WebApp.Entity.Entities.Blogs;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Service
{
    public interface IBlogService : IBaseService<BlogEntity>
    {
        Task<Paging<BlogModel>> GetSearchAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize, string searchText = null);
        Task<BlogModel> AddBlogDetailAsync(BlogModel model);
        Task<BlogModel> UpdateBlogDetailAsync(long blogId, [FromForm] BlogModel model);
    }
}
