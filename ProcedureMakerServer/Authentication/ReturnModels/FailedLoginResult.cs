using ProcedureMakerServer.Interfaces;

namespace ProcedureMakerServer.Authentication.ReturnModels;

public class FailedLoginResult : IMessageResult
{
    public string Message { get; set; } = string.Empty;

    public FailedLoginResult()
    {
        
    }
    public FailedLoginResult(string message)
    {
        Message = message;  
    }
}
