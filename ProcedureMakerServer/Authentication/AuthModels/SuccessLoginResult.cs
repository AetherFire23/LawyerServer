namespace ProcedureMakerServer.Authentication.AuthModels;

public class SuccessLoginResult
{
    public UserDto UserDto { get; set; } = new UserDto();
    public string Token { get; set; } = string.Empty;
}
