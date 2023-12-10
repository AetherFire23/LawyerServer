using DocumentFormat.OpenXml.Packaging;
using ProcedureMakerServer.Dtos;
using System.Text;

namespace ProcedureMakerServer.TemplateManagement.DocumentFillers;

[DocumentFiller(DocumentTypes.PresentationNotice)]
public class PresentationNoticeFiller : DocumentFillerBase
{
    public override string FormatEmailSubjectTitle(CaseDto dto)
    {
        var builder = new StringBuilder();
        builder.Append("NOTIFICATION PAR COURRIEL ");
        builder.Append($"({dto.CourtNumber}) ");
        builder.Append(dto.Plaintiff.LowerCaseFormattedFullName ?? "");
        builder.Append(" c. ");
        builder.Append(dto.Plaintiff.LowerCaseFormattedFullName ?? "");

        var subject = $"NOTIFICATION PAR COURRIEL ({dto.CourtNumber}) {dto.Defender.LowerCaseFormattedFullName} c. {dto.Plaintiff.LowerCaseFormattedFullName}";
        return builder.ToString();
    }

    // expects
    protected override void CreateFixedReplacementKeywords(CaseDto caseDto, List<(string From, string To)> keywordMap, object? additional = null)
    {

    }

    protected override void FillArrayFields(CaseDto caseDto, WordprocessingDocument document, object? additional = null)
    {

        //  Console.WriteLine(additional.GetType());
    }
}
