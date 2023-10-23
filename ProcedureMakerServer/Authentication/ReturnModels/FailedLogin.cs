using ProcedureMakerServer.Enums;
using ProcedureMakerServer.Interfaces;
using Reinforced.Typings.Attributes;

namespace ProcedureMakerServer.Authentication.ReturnModels;

[TsClass]
public class FailedLogin : IMessageResult, IRequestResult
{
    public string Message { get; set; } = string.Empty;

    public string MessageMessage { get; set; } = string.Empty;
    public RequestResultTypes Result { get; } = RequestResultTypes.Fail;

    public FailedLogin()
    {

    }

    public FailedLogin(string message)
    {
        Message = message;
    }
}
