using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.EmailMaker;
using ProcedureMakerServer.Repository;
using ProcedureMakerServer.Scratches;
using ProcedureMakerServer.TemplateManagement;
using ProcedureMakerServer.Utils;

namespace ProcedureMakerServer.Services;

public class NotificationService
{
    private readonly DocumentMakerService _documentMaker;
    private readonly CaseRepository _caseRepository;
    private readonly NotificationEmailSender _notificationEmailSender;

    public NotificationService(DocumentMakerService documentMaker, NotificationEmailSender notificationEmailSender, CaseRepository caseRepository)
    {
        _documentMaker = documentMaker;
        _notificationEmailSender = notificationEmailSender;
        _caseRepository = caseRepository;
    }

    public async Task<string> SendNotificationWithPdfOnly(IFormFile formFile, Guid caseId, string documentName)
    {
        // validate at least that defender and plaintiff exist
        var caseDto = await _caseRepository.MapCaseDto(caseId);
        if (caseDto.Plaintiff is null || caseDto.Defender is null) throw new Exception();

        string pdfDocumentPath = await CopyFormFileToDisk(formFile);

        var credentials = new EmailCredentials() // will put that into appsettings for the app email 
        {
            Email = "richerf3212@gmail.com", // mon email 
            AppPassword = "tbul xjhr odgm wppq",
        };

        var sendEmailInfo = await PrepareEmailInfo(caseDto, credentials, pdfDocumentPath, documentName);

        await _notificationEmailSender.NotifyDocument(credentials, sendEmailInfo);

        string proofOfNotificationPath = await _documentMaker.GenerateProofOfNotificationAsPdf(caseDto,
            sendEmailInfo.Subject,
            credentials);

        return proofOfNotificationPath;
    }

    private static async Task<string> CopyFormFileToDisk(IFormFile formFile)
    {
        string randomFilePath = RandomFilePathMaker.GenerateRandomFilePath("pdf");
        using (Stream fileStream = new FileStream(randomFilePath, FileMode.Create))
        {
            await formFile.CopyToAsync(fileStream);
        }

        return randomFilePath;
    }

    private async Task<SendEmailInfo> PrepareEmailInfo(CaseDto caseDto, EmailCredentials emailCredentials, string pdfDocumentPath, string documentName) // could be a list one day
    {
        var sendingInfo = new SendEmailInfo();
        sendingInfo.Subject = await _documentMaker.GenerateNotificationSubject(caseDto);
        string htmlPath = await _documentMaker.GenerateNotificationBorderAsHtml(caseDto, pdfDocumentPath, documentName);
        sendingInfo.EmailHtmlBody = File.ReadAllText(htmlPath);
        sendingInfo.Tos = await _caseRepository.GetEmailsToNotify(caseDto.Id);
        sendingInfo.PdfFileAttachmentPath = pdfDocumentPath;

        // need to add self as Bcc for notificationReader to work
        sendingInfo.Bccs.Add(emailCredentials.Email);

        return sendingInfo;
    }
}
