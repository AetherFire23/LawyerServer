namespace ProcedureMakerServer.Models;

public class CaseCreationInfo
{
    public Guid LawyerId { get; set; }
    public string CaseNumber { get; set; } = string.Empty;
    public string ClientFirstName {  get; set; } = string.Empty;
    public string ClientLastName {  get; set; } = string.Empty;

    public void Deconstruct(out Guid lawyerId, out string caseNumber, out string clientFirstName, out string clientLastName)
    {
        lawyerId = LawyerId;
        caseNumber = CaseNumber;
        clientFirstName = ClientFirstName;
        clientLastName = ClientLastName;
    }
}
