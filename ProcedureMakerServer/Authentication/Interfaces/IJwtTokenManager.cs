namespace ProcedureMakerServer.Authentication.Interfaces;

public interface IJwtTokenManager
{
    Task<string> GenerateToken(User user);
}