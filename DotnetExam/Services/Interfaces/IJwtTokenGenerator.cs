using System.Security.Claims;
using DotnetExam.Models.Main;
using Microsoft.IdentityModel.Tokens;

namespace DotnetExam.Services.Interfaces;

public interface IJwtTokenGenerator
{
    public string GenerateUserToken(AppUser user, DateTime expiration);
    
    string GenerateFromClaims(IEnumerable<Claim> claims, DateTime expiresAt);
    
    string GenerateToken<T>(T data, DateTime expiresAt);
    
    T? ReadToken<T>(string token, bool shouldThrow = false, TokenValidationParameters? validationParameters = null);
    
    ClaimsPrincipal ReadToken(string token, TokenValidationParameters? validationParameters = null);
    
    TokenValidationParameters CloneParameters();

    RefreshToken GenerateRefreshToken();
    
}