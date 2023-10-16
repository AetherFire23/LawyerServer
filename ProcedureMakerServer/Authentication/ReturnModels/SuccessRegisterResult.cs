using ProcedureMakerServer.Interfaces;

namespace ProcedureMakerServer.Authentication.ReturnModels;

public class SuccessRegisterResult : IMessageResult
{
    public string Message { get; set; } = string.Empty;

    public UserDto User { get; set; }

    public SuccessRegisterResult(UserDto user)
    {
        User = user;
    }
}
