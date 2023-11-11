using ProcedureMakerServer.Dtos;

namespace ProcedureMakerServer.TemplateManagement.DocumentFillers;

[DocumentFiller(DocumentTypes.Backing)]
public class BackingFiller : DocumentFillerBase
{
    public override string FormatEmailSubjectTitle(CaseDto dto)
    {
        return "NOTIFICATION";
    }


    // should use dictionary maybe?
    protected override List<(string From, string To)> GetStaticReplacementKeywords(CaseDto caseDto, object? additional)
    {
        var map = new List<(string From, string To)>();
        map.Add(("lawyerName", $"{caseDto.ManagerLawyer.FirstName}"));
        return map;
    }
}
