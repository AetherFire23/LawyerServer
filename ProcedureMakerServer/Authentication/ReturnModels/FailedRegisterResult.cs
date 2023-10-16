using ProcedureMakerServer.Interfaces;

namespace ProcedureMakerServer.Authentication.ReturnModels;

public class FailedRegisterResult : IMessageResult
{
    public string Message { get; set; } = string.Empty;

    public FailedRegisterResult()
    {
        
    }
    public FailedRegisterResult(string message)
    {
        Message = message;
    }
}
