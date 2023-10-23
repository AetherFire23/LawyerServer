using System.Security.Claims;

namespace ProcedureMakerServer.Authentication.Interfaces;

public interface IJwtTokenManager
{
    Task<string> GenerateToken(User user);
    Task<ClaimsPrincipal> ValidateToken(string token);
}