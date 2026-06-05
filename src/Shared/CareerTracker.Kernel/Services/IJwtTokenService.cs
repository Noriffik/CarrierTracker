using System.Security.Claims;

namespace CareerTracker.Kernel.Services;

public interface IJwtTokenService
{
    string GenerateAccessToken(int userId, string roleClaim, TimeSpan lifetime);
    string GenerateRefreshToken(int userId);
    ClaimsPrincipal? ValidateToken(string token);
}
