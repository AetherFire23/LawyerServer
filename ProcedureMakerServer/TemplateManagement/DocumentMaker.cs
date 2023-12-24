using MailKit.Net.Imap;
using MimeKit;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.EmailMaker;
using ProcedureMakerServer.TemplateManagement.DocumentFillers;
using System.Reflection;
using System.Text;

namespace ProcedureMakerServer.TemplateManagement;

// tips:
// design
// send the widht or height so that I can know 
// what to resize to after 

// html - dynamic css
// 


// also DI these classes instead of static
public class DocumentMaker
{
    // declare as static for caching
    private static Dictionary<DocumentTypes, DocumentFillerBase> _documentsMap = InitializeDocumentFillers();

    public WordDocGenerationInfo GenerateDocxAndFill(CaseDto dto, DocumentTypes documentType, object? additional)
    {
        DocumentFillerBase documentFiller = _documentsMap[documentType];
        WordDocGenerationInfo docxInfo = documentFiller.FillDocument(dto, documentType, additional);
        return docxInfo;
    }

    public async Task<string> GenerateDocumentAsPdf(CaseDto dto, DocumentTypes documentType, object? additional = null)
    {
        WordDocGenerationInfo documentPath = GenerateDocxAndFill(dto, documentType, additional);
        string pdfPath = await WordDocumentExtensions.ConvertToPdf(documentPath);
        return pdfPath;
    }

    public async Task<string> GenerateDocumentAsHtml(CaseDto dto, DocumentTypes documentType, object? additional = null)
    {
        WordDocGenerationInfo documentPath = GenerateDocxAndFill(dto, documentType, additional);
        string htmlPath = await WordDocumentExtensions.ConvertToHtml(documentPath);
        return htmlPath;
    }

    // This will need to change whether or not its about youth, family, etc.
    public async Task<string> GenerateNotificationSubject(CaseDto dto)
    {
        StringBuilder builder = new StringBuilder();
        _ = builder.Append("NOTIFICATION PAR COURRIEL ");
        _ = builder.Append($"({dto.CourtNumber}) ");
        _ = builder.Append(dto.Plaintiff.LowerCaseFormattedFullName ?? string.Empty);
        _ = builder.Append(" c. ");
        _ = builder.Append(dto.Plaintiff.LowerCaseFormattedFullName ?? string.Empty);

        return builder.ToString();
    }

    public async Task<string> GenerateNotificationBorderAsHtml(CaseDto dto, string pdfFilePath, string documentName)
    {
        using PdfDocument pdfDoc = PdfReader.Open(pdfFilePath, PdfDocumentOpenMode.ReadOnly);

        var notificationSlipParams = new NotificationSlipParams()
        {
            PageCount = pdfDoc.Pages.Count,
            DocumentName = documentName,
        };

        string htmlPath = await GenerateDocumentAsHtml(dto, DocumentTypes.TransmissionSlip, notificationSlipParams);
        return htmlPath;
    } 

    public async Task<string> GenerateProofOfNotificationAsPdf(CaseDto dto, string emailSubject, EmailCredentials creds)
    {
        var client = await EmailReceiver.GetClient(creds);
        using MimeMessage emailMessage = await EmailReceiver.FindLastMessageWithTitle(client, emailSubject);
        string pdfPath = await GenerateDocumentAsPdf(dto, DocumentTypes.ProofOfNotification, emailMessage);
        return pdfPath;
    }

    private static Dictionary<DocumentTypes, DocumentFillerBase> InitializeDocumentFillers()
    {
        List<DocumentFillerBase?> documentFillers = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x =>
                CustomAttributeExtensions.GetCustomAttribute<DocumentFillerAttribute>(x) is not null
                && x.IsClass
                && !x.IsAbstract)
            .Select(x => (Activator.CreateInstance(x)) as DocumentFillerBase).ToList();


        var map = new Dictionary<DocumentTypes, DocumentFillerBase>();

        foreach (DocumentFillerBase? documentFiller in documentFillers)
        {
            map.Add(documentFiller.GetType().GetCustomAttribute<DocumentFillerAttribute>().DocumentType, documentFiller);
        }

        return map;
    }
}
