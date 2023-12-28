using MailKit.Net.Imap;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MimeKit;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.EmailMaker;
using ProcedureMakerServer.Services;
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
public class DocumentMakerService
{
    public static Dictionary<DocumentTypes, Type> DocumentToFillerMap = new Dictionary<DocumentTypes, Type>();
    private readonly IServiceProvider _serviceProvider;
    private readonly ProcedureContext _procedureContext;
    public DocumentMakerService(IServiceProvider serviceProvider, ProcedureContext procedureContext)
    {
        _serviceProvider = serviceProvider;
        _procedureContext = procedureContext;
    }

    public async Task<WordDocGenerationInfo> GenerateDocxAndFill(CaseDto dto, DocumentTypes documentType, object? additionalProperties)
    {
        var documentFiller = await this.ResolveDocumentFillerFromType(documentType);
        WordDocGenerationInfo docxInfo = documentFiller.FillDocument(dto, documentType, additionalProperties);
        return docxInfo;
    }

    public async Task<string> GenerateDocumentAsPdf(CaseDto dto, DocumentTypes documentType, object? additional = null)
    {
        WordDocGenerationInfo documentPath = await GenerateDocxAndFill(dto, documentType, additional);
        string pdfPath = await WordDocumentExtensions.ConvertToPdf(documentPath);
        return pdfPath;
    }

    public async Task<string> GenerateDocumentAsHtml(CaseDto dto, DocumentTypes documentType, object? additional = null)
    {
        WordDocGenerationInfo documentPath = await GenerateDocxAndFill(dto, documentType, additional);
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

    private async Task<DocumentFillerBase> ResolveDocumentFillerFromType(DocumentTypes documentType)
    {
        var documentFillerType = DocumentMakerService.DocumentToFillerMap[documentType];
        var documentFiller = _serviceProvider.GetRequiredService(documentFillerType) as DocumentFillerBase;

        return documentFiller;
    }
}

public static class DocumentMakerExtensions
{
    public static void InitializeAndRegisterDocumentFillers(this IServiceCollection serviceCollection)
    {
        var documentFillers = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x =>
                CustomAttributeExtensions.GetCustomAttribute<DocumentFillerAttribute>(x) is not null
                && x.IsClass
                && !x.IsAbstract).ToList();

        foreach (var documentFillerType in documentFillers)
        {
            DocumentTypes documentType = CustomAttributeExtensions.GetCustomAttribute<DocumentFillerAttribute>(documentFillerType).DocumentType;
            DocumentMakerService.DocumentToFillerMap.Add(documentType, documentFillerType);

            serviceCollection.AddScoped(documentFillerType);
        }
    }
}