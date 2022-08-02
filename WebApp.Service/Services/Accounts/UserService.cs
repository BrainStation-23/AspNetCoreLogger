using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Common.Exceptions;
using WebApp.Core.Auths;
using WebApp.Core.Models;
using static WebApp.Entity.Entities.Identities.IdentityModel;

namespace WebApp.Service.Services.Accounts
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IJwtAuth _jwtAuth;

        public UserService(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IJwtAuth jwtAuth)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _jwtAuth = jwtAuth;
        }

        private async Task<bool> CheckPasswordAsync(string password, User user)
        {
            bool isValidPassword = await _userManager.CheckPasswordAsync(user, password);

            return isValidPassword;
        }

        private async Task<List<Claim>> GetClaimsAsync(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new (ClaimTypes.GivenName, $"{user.Firstname} {user.Lastname}"),
                    new (ClaimTypes.Name, user.UserName),
                    new (ClaimTypes.Email, user.Email),
                    new (ClaimTypes.NameIdentifier, user.Id.ToString())
                };

            if (!string.IsNullOrEmpty(user.PhoneNumber))
                authClaims.Add(new Claim(ClaimTypes.MobilePhone, user.PhoneNumber));

            userRoles.ToList().ForEach(r => authClaims.Add(new Claim(ClaimTypes.Role, r)));

            return authClaims;
        }

        public async Task<TokenModel> LoginGenerateJwtTokenAsync(string userName, string password)
        {
            User user = await _userManager.FindByNameAsync(userName);
            var isValidUser = await this.CheckPasswordAsync(password, user);
            if (!isValidUser)
                throw new BadRequestException("Invalid Credentials");

            var userModel = new UserModel
            {
                UserId = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Fullname = $"{user.Firstname} {user.Lastname}"
            };

            List<Claim> authClaims = await this.GetClaimsAsync(user);

            var token = _jwtAuth.GenerateJwtToken(userModel, authClaims);

            return token;
        }

        public async Task<object> AddUserAsync(User user, string password)
        {
            var exists = await _userManager.FindByEmailAsync(user.Email);
            if (exists != null)
                throw new ArgumentException($"User already exists with email {user.Email}");

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                return user;
            }

            throw new AppException($"User already exists with name {user.UserName}", result.Errors);
        }

        public async Task<object> AddRoleAsync(Role role)
        {
            role.StatusId = 1;
            var exists = await _roleManager.FindByNameAsync(role.Name);

            if (exists != null)
                throw new ArgumentException($"Role already exists with name {role.Name}");

            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
                return role;

            throw new AppException($"Role already exists with name {role.Name}", result.Errors);
        }

        public async Task<object> AssignRoleAsync(string username, string role)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                throw new ArgumentNullException(nameof(username), "User doesn't exist.");

            var result = await _userManager.AddToRoleAsync(user, role);

            return result;
        }
    }
}
