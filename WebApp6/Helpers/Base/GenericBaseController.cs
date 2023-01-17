using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApp.Core;
using WebApp.Common.Responses;
using WebApp.Services;

namespace WebApp6.Helpers.Base
{
    [AllowAnonymous]
    public class GenericBaseController<TEntity, TModel> : UserInfoBase where TEntity : class where TModel : class
    {
        protected readonly IBaseService<TEntity, TModel> _service;
        protected readonly IMapper _mapper;

        public GenericBaseController(IBaseService<TEntity, TModel> service, IMapper mapper)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet("page")]
        public virtual async Task<IActionResult> GetPageAsync(int pageIndex = CommonVariables.pageIndex, int pageSize = CommonVariables.pageSize)
        {
            var data = await _service.GetPageAsync(pageIndex, pageSize);

            return new OkResponse(data);
        }

        [HttpGet("find/{id}")]
        public async Task<IActionResult> FindAsync(long id)
        {
            var data = await _service.FindAsync(id);

            return new OkResponse(data);
        }


        [HttpPost("add")]
        public virtual async Task<IActionResult> AddAsync(TModel model)
        {
            var added = await _service.InsertAsync(model);

            return new OkResponse(added);
        }

        [HttpPut("edit/{id}")]
        public virtual async Task<IActionResult> EditAsync(long id, TModel model)
        {
            var updated = await _service.UpdateAsync(id, model);

            return new OkResponse(updated);
        }

        [HttpDelete("delete/{id}")]
        public virtual async Task<IActionResult> DeleteAsync(long id)
        {
            var deleted = await _service.DeleteAsync(id);

            return new OkResponse(deleted);
        }

        [HttpPost("delete")]
        public virtual async Task<IActionResult> DeleteAsync(TModel model)
        {
            Type type = model.GetType();
            long Id = (long)type.GetProperty("Id").GetValue(model);

            var deleted = await _service.DeleteAsync(Id);

            return new OkResponse(deleted);
        }
    }
}
