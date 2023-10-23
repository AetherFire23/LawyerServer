using ProcedureMakerServer.Authentication.AuthModels;

namespace ProcedureMakerServer.Authentication.ReturnModels;

public class LoginResult
{
    public string Token { get; set; } = string.Empty;
    public UserDto UserDto{ get; set; } 
}
