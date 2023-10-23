using OneOf;
using ProcedureMakerServer.Authentication.AuthModels;
using ProcedureMakerServer.Authentication.ReturnModels;
using ProcedureMakerServer.Exceptions;
using ProcedureMakerServer.Models;

namespace ProcedureMakerServer.Authentication.Interfaces;

public interface IAuthManager
{
    Task<OneOf<LoginResult, InvalidCredentials>> GenerateTokenIfCorrectCredentials(LoginRequest loginRequest);
    Task<OneOf<RegisterResult, InvalidRequestMessage>> TryRegister(RegisterRequest registerRequest);
}