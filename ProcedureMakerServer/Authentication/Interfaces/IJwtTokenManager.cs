using Microsoft.Extensions.Options;
using ProcedureMakerServer.Authentication.AuthModels;

namespace ProcedureMakerServer.Authentication.Interfaces;

public interface IJwtTokenManager
{
    Task<string> GenerateToken(User user);
}