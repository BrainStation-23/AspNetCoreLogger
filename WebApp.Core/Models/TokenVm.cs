using Newtonsoft.Json;
using System;

namespace WebApp.Core.Models
{
    public class TokenModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        [JsonProperty("expires_in")]
        public DateTimeOffset TokenExpires { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("refresh_token")] 
        public Guid RefreshToken { get; set; }
        public DateTimeOffset RefreshTokenExpires { get; set; }
    }
}
