using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Models;

namespace ProcedureMakerServer.Exceptions;

public class InvalidCredentials : InvalidRequestMessage
{
    public InvalidCredentials() : base("Invalid Credentials")
    {
    }
}
