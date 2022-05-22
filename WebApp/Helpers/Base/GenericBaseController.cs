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
    public class GenericBaseController<T> : UserInfoBase where T : class
    {
        protected readonly IBaseService<T> _service;

        public GenericBaseController(IBaseService<T> service)
        {
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
        public virtual async Task<IActionResult> AddAsync(T entity)
        {
            var res = await _service.InsertAsync(entity);
            return Created("", res);
        }

        [HttpPut("edit/{id}")]
        public virtual async Task<IActionResult> EditAsync(long id, T entity)
        {
            var res = await _service.UpdateAsync(id, entity);
            return Ok(res);
        }

        [HttpDelete("delete/{id}")]
        public virtual async Task<IActionResult> DeleteAsync(long id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost("delete")]
        public virtual async Task<IActionResult> DeleteAsync(T entity)
        {
            Type type = entity.GetType();
            long Id = (long)type.GetProperty("Id").GetValue(entity);
            await _service.DeleteAsync(Id);
            return NoContent();
        }
    }
}
