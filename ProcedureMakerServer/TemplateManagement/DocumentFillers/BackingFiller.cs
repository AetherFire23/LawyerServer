using ProcedureMakerServer.Dtos;

namespace ProcedureMakerServer.TemplateManagement.DocumentFillers;

[DocumentFiller(DocumentTypes.Backing)]
public class BackingFiller : DocumentFillerBase
{
    // should use dictionary maybe?
    protected override List<(string From, string To)> GetStaticKeywords(CaseDto caseDto)
    {
        var map = new List<(string From, string To)>();
        map.Add(("lawyerName", $"{caseDto.ManagerLawyer.FirstName}"));
        return map;
    }
}
