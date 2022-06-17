using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApp.Service.Services.Accounts;
using WebApp.ViewModels;
using static WebApp.Entity.Entities.Identities.IdentityModel;

namespace WebApp.Controllers.Auths
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public AccountController(IMapper mapper,
            IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Privileged()
        {
            return Ok("Privileged content");
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult PrivilegedById(int id)
        {
            return Ok($"Privileged content {id}");
        }

        [AllowAnonymous]
        [HttpPost("user/add")]
        public async Task<IActionResult> AddUser(UserVm model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model), "request body required.");

            var user = _mapper.Map<User>(model);
            var response = await _userService.AddUserAsync(user, model.Password);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("role/add")]
        public async Task<IActionResult> AddRole(RoleVm model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model), "request body required.");

            var role = _mapper.Map<Role>(model);
            var response = await _userService.AddRoleAsync(role);

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("role/assign")]
        public async Task<IActionResult> AssignRole(UserRoleVm model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model), "request body required.");

            var response = await _userService.AssignRoleAsync(model.Username, model.RoleName);

            return Ok(response);
        }
    }
}
