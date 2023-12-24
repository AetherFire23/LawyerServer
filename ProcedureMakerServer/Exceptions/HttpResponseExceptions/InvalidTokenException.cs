using ProcedureMakerServer.Scratches;
using System.Net;

namespace ProcedureMakerServer.Exceptions.HttpResponseExceptions;

public class InvalidTokenException : HttpExceptionBase
{
    public override HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;

    public InvalidTokenException()
    {
        base.HttpMessage = "The provided token was invalid";
    }
}
