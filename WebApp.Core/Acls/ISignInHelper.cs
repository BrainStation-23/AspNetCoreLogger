using System;
using System.Collections.Generic;

namespace WebApp.Core.Acls
{
    public interface ISignInHelper
    {
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