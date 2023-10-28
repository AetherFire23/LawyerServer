using ProcedureMakerServer.Scratches;
using System.Net;

namespace ProcedureMakerServer.Exceptions.HttpResponseExceptions;

public class InvalidTokenException : MyInvalidExceptionBase
{
    public override HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;

    public InvalidTokenException()
    {
        base.Message = "The provided token was invalid";
    }
}
