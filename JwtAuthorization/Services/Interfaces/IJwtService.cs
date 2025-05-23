﻿using JwtAuthorization.Models.Configuration;
using JwtAuthorization.Models.Databases;
using JwtAuthorization.Models.DTO.Request;

namespace JwtAuthorization.Services.Interfaces
{
    public interface IJwtService
    {
        Task<AuthResult> GenerateJwtToken(User user, List<UserRole> userRoles);

        bool VerifyToken(TokenRequest tokenRequest, out string account);

        Task<AuthResult> VerifyAndGenerateToken(TokenRequest tokenRequest);
    }
}
