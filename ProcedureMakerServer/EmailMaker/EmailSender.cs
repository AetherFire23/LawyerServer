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

        //string html = File.ReadAllText(htmlPath2);
        
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = sendEmailInfo.EmailHtmlBody};


        // send email
        using var smtp = new SmtpClient();
        smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        smtp.Authenticate(credentials.Email, credentials.AppPassword);
        smtp.Send(email);
        smtp.Disconnect(true);


        // recolter la preuve de notification

        //using (var client = new ImapClient())
        //{
        //    client.Connect("imap.gmail.com", 993, true);

        //    client.Authenticate("richerf3212@gmail.com", "dfdb aybg gjlm lxor");

        //    // The Inbox folder is always available on all IMAP servers...
        //    IMailFolder inbox = client.Inbox;
        //    inbox.Open(FolderAccess.ReadOnly);

        //    Console.WriteLine("Total messages: {0}", inbox.Count);
        //    Console.WriteLine("Recent messages: {0}", inbox.Recent);


        //    IList<UniqueId> uids = inbox.Search(SearchQuery.SubjectContains("NOTIFICATION"));

        //    var messages = inbox.GetMessagesWithId(uids);

        //    Console.WriteLine(random);
        //    var messagesWithMostRecentDate = messages.First(x => x.Date.Equals(messages.Max(x => x.Date)));

        //    client.Disconnect(true);
        //}


    }
}
