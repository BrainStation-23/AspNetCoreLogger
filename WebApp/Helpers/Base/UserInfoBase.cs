using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace WebApp.Helpers.Base
{
    public class UserInfoBase : ControllerBase
    {
        public long? UserId
        {
            get => User.Identity?.IsAuthenticated ?? false ? long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) : null;
        }

        public string Email
        {
            get => User.Identity?.IsAuthenticated ?? false ? User.FindFirstValue(ClaimTypes.Email) : null;
        }

        public string MobileNumber
        {
            get => User.Identity?.IsAuthenticated ?? false ? User.FindFirstValue(ClaimTypes.MobilePhone) : null;
        }

        public string Username
        {
            get => User.Identity?.IsAuthenticated ?? false ? User.Identity.Name : null;
        }

        public string UserFullname
        {
            get => User.Identity?.IsAuthenticated ?? false ? User.FindFirstValue(ClaimTypes.GivenName) : null;
        }

        public List<string> Roles
        {
            get
            {
                var roles = new List<string>();

                if (User.Identity == null)
                    return roles;

                if (User.Identity.IsAuthenticated)
                    return User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

                return roles;
            }
        }
    }
}
