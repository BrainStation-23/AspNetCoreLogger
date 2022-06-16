using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApp.Core;
using WebApp.Core.Responses;
using WebApp.Services;

namespace WebApp.Helpers.Base
{
    [AllowAnonymous]
    public class GenericBaseController<TEntity, TDto> : UserInfoBase where TEntity : class where TDto : class
    {
        protected readonly IBaseService<TEntity, TDto> _service;
        protected readonly IMapper _mapper;

        public GenericBaseController(IBaseService<TEntity, TDto> service, IMapper mapper)
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
            return Ok(await _service.FindAsync(id));
        }


        [HttpPost("add")]
        public virtual async Task<IActionResult> AddAsync(TDto model)
        {
            var res = await _service.InsertAsync(model);
            return Created("", res);
        }

        [HttpPut("edit/{id}")]
        public virtual async Task<IActionResult> EditAsync(long id, TDto model)
        {
            var res = await _service.UpdateAsync(id, model);
            return Ok(res);
        }

        [HttpDelete("delete/{id}")]
        public virtual async Task<IActionResult> DeleteAsync(long id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("delete")]
        public virtual async Task<IActionResult> DeleteAsync(TDto model)
        {
            Type type = model.GetType();
            long Id = (long)type.GetProperty("Id").GetValue(model);
            await _service.DeleteAsync(Id);
            return NoContent();
        }
    }
}
