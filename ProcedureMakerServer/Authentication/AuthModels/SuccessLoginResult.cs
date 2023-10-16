namespace ProcedureMakerServer.Authentication.AuthModels;

public class SuccessLoginResult
{
    public User User { get; set; }
    public string Token { get; set; } = string.Empty;
}
