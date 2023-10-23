﻿using ProcedureMakerServer.Enums;
using ProcedureMakerServer.Interfaces;

namespace ProcedureMakerServer.Authentication.ReturnModels;

public class FailedRegisterResult : IMessageResult, IRequestResult
{
    public string Message { get; set; } = string.Empty;

    public RequestResultTypes Result { get; } = RequestResultTypes.Fail;

    public FailedRegisterResult()
    {

    }
    public FailedRegisterResult(string message)
    {
        Message = message;
    }
}