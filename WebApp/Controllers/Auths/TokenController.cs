using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApp.Service.Services.Accounts;
using WebApp.ViewModels;

namespace WebApp.Controllers.Auths
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IUserService _userService;

        public TokenController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateTokenFromBody([FromBody] LoginVm model)
        {
            var token = await _userService.LoginGenerateJwtTokenAsync(model.UserName, model.Password);
            if (token == null)
                throw new ArgumentException("can't generate token.");

            return Ok(token);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateTokenFromForm([FromForm] LoginVm model)
        {
            var token = await _userService.LoginGenerateJwtTokenAsync(model.UserName, model.Password);
            if (token == null)
                throw new ArgumentException("can't generate token.");

            return Ok(token);
        }
    }
}
