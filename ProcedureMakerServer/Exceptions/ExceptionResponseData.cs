using Reinforced.Typings.Attributes;

namespace ProcedureMakerServer.Exceptions;

[TsClass]
public class ExceptionResponseData
{
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; } = null;
}
