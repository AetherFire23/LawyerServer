﻿using ProcedureMakerServer.Exceptions.HttpResponseExceptions;
using System.Net;

namespace ProcedureMakerServer.Scratches;

public abstract class MyInvalidExceptionBase : Exception, IHandledException
{
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; } = null;

    public abstract HttpStatusCode StatusCode { get; set; }

    public MyInvalidExceptionBase()
    {

    }

    public MyInvalidExceptionBase(object? obj)
    {
        this.Data = obj;
    }
}