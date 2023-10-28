using Reinforced.Typings.Attributes;

namespace ProcedureMakerServer.Authentication.ReturnModels;

[TsClass]
public class LoginResult
{
    public string Token { get; set; } = string.Empty;
    public UserDto UserDto { get; set; }
}
