using ProcedureMakerServer.Interfaces;

namespace ProcedureMakerServer.Models;

public class InvalidRequestMessage : IMessageResult
{
	public string Message { get; set; } = string.Empty;

	public InvalidRequestMessage(string message)
	{
		Message = message;
	}
}
