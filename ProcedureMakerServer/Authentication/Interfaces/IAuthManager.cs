using OneOf;
using ProcedureMakerServer.Authentication.AuthModels;
using ProcedureMakerServer.Authentication.ReturnModels;

namespace ProcedureMakerServer.Authentication.Interfaces;

public interface IAuthManager
{
    Task<OneOf<FailedLoginResult, SuccessLoginResult>> GenerateTokenIfCorrectCredentials(LoginRequest loginRequest);
    Task<OneOf<FailedRegisterResult, SuccessRegisterResult>> TryRegister(RegisterRequest registerRequest);
}