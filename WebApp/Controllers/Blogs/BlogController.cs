using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApp.Core;
using WebApp.Core.Responses;
using WebApp.Helpers.Base;
using WebApp.Service;
using WebApp.Entity.Entities.Blogs;
using WebApp.Service.Models.Blogs;
using System;
using WebApp.Core.Exceptions;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogController : GenericBaseController<BlogEntity>
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService) : base(blogService)
        {
            _blogService = blogService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetSearchAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize, string searchText = null)
        {
            var res = await _blogService.GetSearchAsync(pageIndex, pageSize, searchText);

            return new OkResponse(res);
        }

        [HttpPost()]
        public async Task<IActionResult> AddBlogDetailAsync(BlogModel model)
        {
            var res = await _blogService.AddBlogDetailAsync(model);

            return new OkResponse(res);
        }

        [HttpPut("{blogId}")]
        public async Task<IActionResult> UpdateBlogDetailAsync(long blogId, [FromForm] BlogModel model)
        {
            if (blogId == 1)
                throw new ArgumentException("error argument");

            if (blogId == 2)
                throw new BadRequestException("error argument");

            if (blogId == 3)
                throw new NotFoundException("error argument");

            var res = await _blogService.UpdateBlogDetailAsync(blogId, model);

            return new OkResponse(res);
        }
    }
}
