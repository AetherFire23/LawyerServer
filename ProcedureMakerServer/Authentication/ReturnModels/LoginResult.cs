using ProcedureShared.Authentication;

namespace ProcedureMakerServer.Authentication.ReturnModels;


public class LoginResult
{
	public string Token { get; set; } = string.Empty;
	public UserDto? UserDto { get; set; }
}
