using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using ProcedureMakerServer.Dtos;

namespace ProcedureMakerServer.TemplateManagement.DocumentFillers;

[DocumentFiller(DocumentTypes.TransmissionSlip)]
public class TransmissionSlipFiller : DocumentFillerBase
{
    public override string FormatEmailSubjectTitle(CaseDto dto)
    {
        l.Add(("dayMonthYear", email.Date.ToString("dd/MM/yyyy", fr)));

        return "";
    }

    
    protected override List<(string From, string To)> GetStaticReplacementKeywords(CaseDto caseDto, object? additional)
    {
        ArgumentNullException.ThrowIfNull(additional);

        NotificationSlipParams additionalParams = additional as NotificationSlipParams;
        List<(string From, string To)> pairs = new();
        pairs.Add(("PageCount", additionalParams.PageCount.ToString()));

        return pairs;
    }
}

public class NotificationSlipParams
{
    public int PageCount { get; set; }
}