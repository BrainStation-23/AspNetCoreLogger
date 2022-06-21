using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApp.Core.Models;

namespace WebApp.Core.Auths
{
    public class JwtAuth : IJwtAuth
    {
        private readonly JwtOption _jwtOption;

        public JwtAuth(IOptionsSnapshot<JwtOption> jwtOption)
        {
            this._jwtOption = jwtOption.Value;
        }

        public TokenModel GenerateJwtToken(UserModel user, List<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_jwtOption.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Audience = _jwtOption.Audience,
                Issuer = _jwtOption.Issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new TokenModel
            {
                Username = user.Username,
                AccessToken = tokenString,
                TokenExpires = token.ValidTo,
                TokenType = "Bearer"
            };
        }
    }
}
