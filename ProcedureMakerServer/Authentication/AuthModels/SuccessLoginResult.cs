namespace ProcedureMakerServer.Authentication.AuthModels;

public class SuccessLoginResult
{
    public UserDto User { get; set; }
    public string Token { get; set; } = string.Empty;
}
