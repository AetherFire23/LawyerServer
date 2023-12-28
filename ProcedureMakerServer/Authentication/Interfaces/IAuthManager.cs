using ProcedureMakerServer.Authentication.AuthModels;
using ProcedureMakerServer.Authentication.ReturnModels;

namespace ProcedureMakerServer.Authentication.Interfaces;

public interface IAuthManager
{
	Task<LoginResult> GenerateTokenIfCorrectCredentials(LoginRequest loginRequest);

	Task TryRegister(RegisterRequest registerRequest);
}