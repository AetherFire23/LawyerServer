using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.EmailMaker;
using ProcedureMakerServer.Repository;
using ProcedureMakerServer.Scratches;
using ProcedureMakerServer.TemplateManagement;
using ProcedureMakerServer.TemplateManagement.PdfManagement;
using ProcedureMakerServer.Utils;

namespace ProcedureMakerServer.Services;

public class NotificationService
{
    private readonly DocumentMaker _documentMaker;
    private readonly CaseRepository _caseRepository;
    private readonly NotificationEmailSender _notificationEmailSender;

    public NotificationService(DocumentMaker documentMaker, NotificationEmailSender notificationEmailSender, CaseRepository caseRepository)
    {
        _documentMaker = documentMaker;
        _notificationEmailSender = notificationEmailSender;
        _caseRepository = caseRepository;
    }

    public async Task<string> SendNotificationWithPdfOnly(IFormFile formFile, Guid caseId, string documentName)
    {
        string pdfDocumentPath = await CopyFormFileToDisk(formFile);
        var caseDto = await _caseRepository.MapCaseDto(caseId);

        var credentials = new EmailCredentials() // will put that into appsettings for the app email 
        {
            Email = "sakamafaka@gmail.com", // mon email 
            AppPassword = MyPassword.Pass,
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

    // I Should be able to Save the defaultCCs I guess
    // no Bccs either rn 
    async Task<SendEmailInfo> PrepareEmailInfo(CaseDto caseDto, EmailCredentials emailCredentials, string pdfDocumentPath, string documentName) // could be a list one day
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
