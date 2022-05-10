using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace WebApp.Core.Acls
{
    public class SignInHelper : ISignInHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SignInHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            if (_httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated ?? false)
            {
                var user = _httpContextAccessor.HttpContext.User;
                UserId = long.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
                Email = user.FindFirstValue(ClaimTypes.Email);
                MobileNumber = user.FindFirstValue(ClaimTypes.MobilePhone);
                Fullname = user.FindFirstValue(ClaimTypes.GivenName);
                Username = user.Identity.Name;
                IsAuthenticated = user.Identity.IsAuthenticated;
                Roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
                JwtExpiresAt = DateTimeOffset.FromUnixTimeSeconds(long.Parse(user.FindFirstValue("exp")));
            }

            var request = _httpContextAccessor?.HttpContext?.Request;

            AccessToken = request?.Headers["Authorization"];
            RequestOrigin = request?.Headers["Origin"].ToString()?.Trim();
            AccessToken = AccessToken != "null" ? AccessToken?.Split(" ")[1] : default;
        }

        public long? UserId { get; }
        public string Email { get; }
        public string Fullname { get; }
        public string MobileNumber { get; }
        public string Username { get; }

        public List<string> Roles { get; }

        public bool IsAuthenticated { get; }
        public string AccessToken { get; }
        public DateTimeOffset JwtExpiresAt { get; }
        public string RequestOrigin { get; }
    }
}
