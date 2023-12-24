namespace ProcedureMakerServer.Models;

public class CaseCreationInfo
{
    public Guid ClientId { get; set; }
    public string CaseNumber { get; set; } = string.Empty;

    public void Deconstruct(out Guid clientId, out string caseNumber)
    {
        caseNumber = CaseNumber;
        clientId = ClientId;
    }
}
