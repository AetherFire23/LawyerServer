using DocumentFormat.OpenXml.Packaging;
using MimeKit;
using ProcedureMakerServer.Dtos;

namespace ProcedureMakerServer.TemplateManagement.DocumentFillers;

[DocumentFiller(DocumentTypes.PresentationNotice)]
public class PresentationNoticeFiller : DocumentFillerBase
{
    protected override List<(string From, string To)> GetStaticReplacementKeywords(CaseDto caseDto, object? additional = null)
    {
        List<(string From, string To)> keywords = new();

        keywords.Add(("lawyerName", caseDto.Client.FirstName));

        return keywords;
    }

    protected override void FillArrayFields(CaseDto caseDto, WordprocessingDocument document, object? additional = null)
    {


      //  Console.WriteLine(additional.GetType());
    }
}
