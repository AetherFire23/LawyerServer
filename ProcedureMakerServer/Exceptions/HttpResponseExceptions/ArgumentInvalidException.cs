using ProcedureMakerServer.Scratches;
using System.Net;

namespace ProcedureMakerServer.Exceptions.HttpResponseExceptions;



public class ArgumentInvalidException : HttpExceptionBase
{
	public override HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;

	public ArgumentInvalidException(string message)
	{
		base.HttpMessage = message;
	}
}
