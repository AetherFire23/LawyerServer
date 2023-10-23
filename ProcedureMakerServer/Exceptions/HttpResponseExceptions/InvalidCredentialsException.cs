using ProcedureMakerServer.Scratches;

namespace ProcedureMakerServer.Exceptions.HttpResponseExceptions;

public class InvalidCredentialsException : MyInvalidExceptionBase
{
    public override int StatusCode { get; set; } = 400;


    public InvalidCredentialsException()
    {
        base.Message = "The provided credentials were invalid.";
    }
}
