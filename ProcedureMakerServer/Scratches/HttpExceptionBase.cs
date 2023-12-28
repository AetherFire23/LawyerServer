using ProcedureMakerServer.Exceptions.HttpResponseExceptions;
using System.Net;

namespace ProcedureMakerServer.Scratches;

public abstract class HttpExceptionBase : Exception, IHandledException
{
	public string HttpMessage { get; set; } = string.Empty;
	public object? ExceptionData { get; set; } = null;

	public abstract HttpStatusCode StatusCode { get; set; }

	public HttpExceptionBase()
	{

	}

	public HttpExceptionBase(object? obj)
	{
		this.ExceptionData = obj;
	}
}
