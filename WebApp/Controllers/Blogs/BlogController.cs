using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Threading.Tasks;
using WebApp.Core;
using WebApp.Core.Responses;
using WebApp.Entity.Entities.Blogs;
using WebApp.Helpers.Base;
using WebApp.Service;
using WebApp.Service.Contract.Models.Blogs;
using WebApp.Swaggers.Examples.Requests.Blogs;
using WebApp.Swaggers.Examples.Responses.Blogs;

namespace WebApp.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BlogController : GenericBaseController<BlogEntity, BlogModel>
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService, IMapper mapper) : base(blogService, mapper)
        {
            _blogService = blogService;
        }

        [HttpGet("search")]
        [Produces("application/json")]
        public async Task<IActionResult> GetSearchAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize, string searchText = null)
        {
            var res = await _blogService.GetSearchAsync(pageIndex, pageSize, searchText);

            return new OkResponse(res);
        }

        [SwaggerRequestExample(typeof(BlogModel), typeof(BlogRequestExample))]
        [SwaggerResponse(200, Type = typeof(BlogModel), Description = "successful...")]
        [SwaggerResponseExample(200, typeof(BlogResponseExample))]
        [SwaggerResponse(404, Type = typeof(BlogModel), Description = "failed...")]
        [SwaggerResponseExample(404, typeof(BlogResponse404Example))]
        [ProducesResponseType(typeof(BlogModel), 200)]
        [HttpPost()]
        public async Task<IActionResult> AddBlogDetailAsync(BlogModel model)
        {
            var res = await _blogService.AddBlogDetailAsync(model);

            return new OkResponse(res);
        }

        [HttpPut("{blogId}")]
        public async Task<IActionResult> UpdateBlogDetailAsync(long blogId, [FromForm] BlogModel model)
        {
            var res = await _blogService.UpdateBlogDetailAsync(blogId, model);

            return new OkResponse(res);
        }
    }
}
