using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp.Core;
using WebApp.Common.Collections;
using WebApp.Entity.Entities.Blogs;
using WebApp.Service.Contract.Models.Blogs;
using WebApp.Services;

namespace WebApp.Service
{
    public interface IBlogService : IBaseService<BlogEntity, BlogModel>
    {
        Task<Paging<BlogModel>> GetSearchAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize, string searchText = null);
        Task<BlogModel> AddBlogDetailAsync(BlogModel model);
        Task<BlogModel> UpdateBlogDetailAsync(long blogId, [FromForm] BlogModel model);

        Task<List<BlogModel>> GetBlogsSpAsync();
    }
}
