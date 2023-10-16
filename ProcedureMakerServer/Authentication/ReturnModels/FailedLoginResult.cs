using ProcedureMakerServer.Interfaces;

namespace ProcedureMakerServer.Authentication.ReturnModels;

public class FailedLoginResult : IMessageResult
{
    public string Message { get; set; } = string.Empty;
}
