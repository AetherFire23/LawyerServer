using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using MailKit;
using MimeKit;
using MailKit.Net.Smtp;

namespace ProcedureMakerServer.EmailMaker;

public static class EmailSender
{


    // will need to make different senders for gmail / /outlook etc cos diff servers. 
    public static async Task NotifyDocument(EmailCredentials credentials, SendEmailInfo sendEmailInfo)
    {
        // create email message
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(credentials.Email));
        email.To.Add(MailboxAddress.Parse(sendEmailInfo.To));
        email.Subject = sendEmailInfo.Subject;

        var add = InternetAddress.Parse("balthazardf@hotmail.com");
        email.Bcc.Add(add);


        // create our message text, just like before (except don't set it as the message.Body)
        var body = new TextPart("html")
        {
            Text = sendEmailInfo.EmailHtmlBody,
        };

        // create an image attachment for the file located at path
        var attachment = new MimePart("application", "pdf")
        {
            Content = new MimeContent(File.OpenRead(sendEmailInfo.PdfAttachmentPath)),
            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
            ContentTransferEncoding = ContentEncoding.Base64,
            FileName = Path.GetFileName(sendEmailInfo.PdfAttachmentPath)
        };

        // now create the multipart/mixed container to hold the message text and the
        // image attachment
        var multipart = new Multipart("mixed");
        multipart.Add(body);
        multipart.Add(attachment);


        email.Body = multipart;
        using var smtp = new SmtpClient();
        smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        smtp.Authenticate(credentials.Email, credentials.AppPassword);
        smtp.Send(email);
        smtp.Disconnect(true);
    }
}
