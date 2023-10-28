﻿using ProcedureMakerServer.Scratches;
using System.Net;

namespace ProcedureMakerServer.Exceptions.HttpResponseExceptions;

public class InvalidCredentialsException : MyInvalidExceptionBase
{
    public override HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;

    public InvalidCredentialsException()
    {
        base.Message = "The provided credentials were invalid.";
    }
}
