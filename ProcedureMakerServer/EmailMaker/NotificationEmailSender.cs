using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using ProcedureMakerServer.Extensions;

namespace ProcedureMakerServer.EmailMaker;

public class NotificationEmailSender
{
    public async Task NotifyDocument(EmailCredentials credentials, SendEmailInfo sendEmailInfo)
    {
        var mimeMessage = PrepareEmail(credentials, sendEmailInfo);
        SendEmail(mimeMessage, credentials);
    }

    private void SendEmail(MimeMessage mimeMessage, EmailCredentials credentials)
    {
        var smtpClient = new SmtpClient();
        smtpClient.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        smtpClient.Authenticate(credentials.Email, credentials.AppPassword);
        smtpClient.Send(mimeMessage);
        smtpClient.Disconnect(true);
    }

    private MimeMessage PrepareEmail(EmailCredentials credentials, SendEmailInfo sendEmailInfo)
    {
        var mimeMessage = new MimeMessage();

        ConfigureEmailHeaders(mimeMessage, credentials, sendEmailInfo);

        // create our message text, just like before (except don't set it as the message.Body)
        var textPartHtmlBody = new TextPart("html")
        {
            Text = sendEmailInfo.EmailHtmlBody,
        };

        // create an image attachment for the file located at path
        var mimePdfAttachment = new MimePart("application", "pdf")
        {
            Content = new MimeContent(File.OpenRead(sendEmailInfo.PdfFileAttachmentPath)),
            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
            ContentTransferEncoding = ContentEncoding.Base64,
            FileName = Path.GetFileName(sendEmailInfo.PdfFileAttachmentPath)
        };

        // now create the multipart/mixed container to hold both 1) the message text and 2) the image attachment
        var multipart = new Multipart("mixed");
        multipart.Add(textPartHtmlBody);
        multipart.Add(mimePdfAttachment);
        mimeMessage.Body = multipart;

        return mimeMessage;
    }

    private void ConfigureEmailHeaders(MimeMessage mimeMessage, EmailCredentials credentials, SendEmailInfo sendEmailInfo)
    {
        mimeMessage.From.Add(MailboxAddress.Parse(credentials.Email));
        mimeMessage.AddTos(sendEmailInfo.Tos);
        mimeMessage.AddCc(sendEmailInfo.Cc);
        mimeMessage.AddBcc(sendEmailInfo.Bccs);
        mimeMessage.Subject = sendEmailInfo.Subject;
    }
}
