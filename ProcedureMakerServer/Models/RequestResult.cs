using ProcedureMakerServer.Enums;
namespace ProcedureMakerServer.Models;

public class RequestResult
{
    public string MessageProcessingHandler { get; set; }
    public RequestResultTypes Result { get; } = RequestResultTypes.Fail;
    public string SerializedData { get; } = string.Empty;

    public RequestResult(RequestResultTypes result)
    {
        
    }
    public RequestResult(RequestResultTypes result, string serializedData)
    {
        Result = result;
        SerializedData = serializedData;
    }
}