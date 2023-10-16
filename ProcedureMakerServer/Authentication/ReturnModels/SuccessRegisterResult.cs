using ProcedureMakerServer.Interfaces;

namespace ProcedureMakerServer.Authentication.ReturnModels;

public class SuccessRegisterResult : IMessageResult
{
    public string Message { get; set; } = string.Empty;

    public User User { get; set; }

    public SuccessRegisterResult(User user)
    {
        User = user;
    }
}
