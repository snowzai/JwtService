﻿namespace JwtAuthorization.Models.Configuration
{
    public class AuthResult
    {
        public string Token { get; set; }

        public bool Result { get; set; }

        public List<string> Errors { get; set; }

        public string RefreshToken { get; set; }
    }
}
