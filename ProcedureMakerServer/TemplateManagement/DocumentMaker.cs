using DocumentFormat.OpenXml.Packaging;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.TemplateManagement.DocumentFillers;
using ProcedureMakerServer.TemplateManagement.PdfManagement;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ProcedureMakerServer.TemplateManagement;

public static class DocumentMaker
{
    private static Dictionary<DocumentTypes, DocumentFillerBase> _documentsMap
        = InitializeDocumentFillers();

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

    public static WordprocessingDocument GenerateDocument(CaseDto dto, DocumentTypes documentType)
    {
        DocumentFillerBase documentFiller = _documentsMap[documentType];
        WordprocessingDocument doc = documentFiller.GenerateDocument(dto);
        return doc;
    }

    public static async Task<string> GenerateDocumentAsPdf(CaseDto dto, DocumentTypes documentType)
    {
        DocumentFillerBase documentFiller = _documentsMap[documentType];
        WordprocessingDocument doc = documentFiller.GenerateDocument(dto);

        string random = CreateRandomPath();
        string randomFileWithExtension = (random) + ".docx";
        string absolutePath = @$"C:\Program Files\LibreOffice\program\{randomFileWithExtension}";

        OpenXmlPackage package = doc.Clone(absolutePath, true);
        package.Dispose();
        doc.Dispose();

        await DocxToPdfConverter.CreatePdf(absolutePath, "test");
        string pdfPath = $@"test\{random}.pdf";
        return pdfPath;
    }

    public static async Task<string> GenerateDocumentAsHtml(CaseDto dto, DocumentTypes documentType)
    {
        DocumentFillerBase documentFiller = _documentsMap[documentType];
        WordprocessingDocument doc = documentFiller.GenerateDocument(dto);
        string random = CreateRandomPath();
        string randomFileWithExtension = (random) + ".docx";
        string absolutePath = @$"C:\Program Files\LibreOffice\program\{randomFileWithExtension}";

        OpenXmlPackage package = doc.Clone(absolutePath, true);
        package.Dispose();
        doc.Dispose();

        await DocxToHtmlConverer.ConvertToHtml(absolutePath, "test");
        string pdfPath = $@"test\{random}.html";
        return pdfPath;
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
        int numberOfPages = pdfDoc.Pages.Count;

        DocumentFillerBase documentFiller = _documentsMap[DocumentTypes.NotificationSlip] as NotificationSlipFiller;
        WordprocessingDocument docxDoc = documentFiller.GenerateDocument();

    }

    private static string CreateRandomPath()
    {
        string randomPath = Guid.NewGuid().ToString().Replace("-", string.Empty);

        return randomPath;
    }
}
