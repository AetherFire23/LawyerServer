﻿using ProcedureMakerServer.Scratches;
using System.Net;

namespace ProcedureMakerServer.Exceptions.HttpResponseExceptions;

public class InvalidCredentialsException : HttpExceptionBase
{
	public override HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;

	public InvalidCredentialsException()
	{
		base.HttpMessage = "The provided credentials were invalid.";
	}
}
