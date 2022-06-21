using System.Collections.Generic;
using System.Security.Claims;
using WebApp.Core.Models;

namespace WebApp.Core.Auths
{
    public interface IJwtAuth
    {
        TokenModel GenerateJwtToken(UserModel user, List<Claim> claims);
    }
}