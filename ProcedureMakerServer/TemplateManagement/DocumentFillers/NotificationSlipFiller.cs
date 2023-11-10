using DocumentFormat.OpenXml.Packaging;
using ProcedureMakerServer.Dtos;

namespace ProcedureMakerServer.TemplateManagement.DocumentFillers;

public class NotificationSlipFiller : DocumentFillerBase
{
    public override string FormatEmailSubjectTitle(CaseDto dto)
    {

        return "Notifiation";
    }

    protected override List<(string From, string To)> GetStaticReplacementKeywords(CaseDto caseDto)
    {
        List<(string From, string To)> pairs = new();

        return pairs;
    }
}
