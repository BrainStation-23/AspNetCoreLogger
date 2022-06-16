﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Net;
using System.Threading.Tasks;
using WebApp.Core;
using WebApp.Core.Responses;
using WebApp.Entity.Entities.Blogs;
using WebApp.Examples.Requests;
using WebApp.Examples.Responses;
using WebApp.Helpers.Base;
using WebApp.Service;
using WebApp.Service.Contract.Models;
using WebApp.Service.Models.Blogs;

namespace WebApp.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class BlogController : GenericBaseController<BlogEntity, BlogDto>
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

        [SwaggerRequestExample(typeof(BlogDto), typeof(BlogRequestExample))]
        [SwaggerResponse(200, Type = typeof(BlogDto), Description = "successful...")]
        [SwaggerResponseExample(200, typeof(BlogResponseExample))]
        [SwaggerResponse(404, Type = typeof(BlogDto), Description = "failed...")]
        [SwaggerResponseExample(404, typeof(BlogResponse404Example))]
        //[ProducesResponseType(typeof(BlogDto), 200)]
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


        [HttpPut("exception")]
        public IActionResult Exception()
        {
            throw new ArgumentException("My data not found in data store.");
        }
    }
}
