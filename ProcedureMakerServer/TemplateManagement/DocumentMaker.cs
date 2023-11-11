using DocumentFormat.OpenXml.Packaging;
using MailKit.Net.Imap;
using MimeKit;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.EmailMaker;
using ProcedureMakerServer.TemplateManagement.DocumentFillers;
using System.Reflection;

namespace ProcedureMakerServer.TemplateManagement;

public static class DocumentMaker
{
    private static Dictionary<DocumentTypes, DocumentFillerBase> _documentsMap
        = InitializeDocumentFillers();

    public static WordprocessingDocument GenerateDocument(CaseDto dto, DocumentTypes documentType)
    {
        DocumentFillerBase documentFiller = _documentsMap[documentType];
        WordprocessingDocument doc = documentFiller.GenerateDocument(dto, documentType);
        return doc;
    }

    public static async Task<string> GenerateDocumentAsPdf(CaseDto dto, DocumentTypes documentType, object? additional = null)
    {
        DocumentFillerBase documentFiller = _documentsMap[documentType];
        using WordprocessingDocument doc = documentFiller.GenerateDocument(dto, documentType, additional);
        string pdfPath = await doc.ConvertToPdf();
        return pdfPath;
    }

    public static async Task<string> GenerateDocumentAsHtml(CaseDto dto, DocumentTypes documentType, object? additional = null)
    {
        DocumentFillerBase documentFiller = _documentsMap[documentType];
        using WordprocessingDocument doc = documentFiller.GenerateDocument(dto, documentType, additional);
        string htmlPath = await doc.ConvertToHtml();
        return htmlPath;
    }

    public static async Task<string> GenerateEmailSubject(CaseDto dto, DocumentTypes documentTypes)
    {
        DocumentFillerBase documentFiller = _documentsMap[documentTypes];
        string subject = documentFiller.FormatEmailSubjectTitle(dto);

        return subject;
    }

    public static async Task<string> GenerateNotificationBorderAsHtml(CaseDto dto, string signedDocumentPdfPath)
    {
        using PdfDocument pdfDoc = PdfReader.Open(signedDocumentPdfPath, PdfDocumentOpenMode.ReadOnly);
        var notificationSlipParams = new NotificationSlipParams()
        {
            PageCount = pdfDoc.Pages.Count
        };
        string htmlPath = await GenerateDocumentAsHtml(dto, DocumentTypes.TransmissionSlip, notificationSlipParams);
        return htmlPath;
    }

    public static async Task<string> GenerateProofOfNotificationAsPdf(CaseDto dto,
        string emailSubject,
        EmailCredentials creds)
    {
        ImapClient client = await EmailReceiver.GetClient(creds);
        using MimeMessage emailMessage = await EmailReceiver.FindLastMessageWithTitle(client, emailSubject);

        string pdfPath = await DocumentMaker.GenerateDocumentAsPdf(dto, DocumentTypes.ProofOfNotification, emailMessage);
        return pdfPath;
    }

    private static Dictionary<DocumentTypes, DocumentFillerBase> InitializeDocumentFillers()
    {
        var documentFillers = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => CustomAttributeExtensions.GetCustomAttribute<DocumentFillerAttribute>(x) is not null
                && x.IsClass && !x.IsAbstract)
            .Select(x => (Activator.CreateInstance(x)) as DocumentFillerBase).ToList();


        var map = new Dictionary<DocumentTypes, DocumentFillerBase>();

        foreach (var documentFiller in documentFillers)
        {
            map.Add(documentFiller.GetType().GetCustomAttribute<DocumentFillerAttribute>().DocumentType, documentFiller);
        }

        return map;
    }
}
