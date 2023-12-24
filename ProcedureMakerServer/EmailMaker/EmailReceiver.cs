using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using ProcedureMakerServer.Utils;

namespace ProcedureMakerServer.EmailMaker;

public static class EmailReceiver
{

    /// <summary> return an IDisposable, remember </summary>
    public static async Task<ImapClient> GetClient(EmailCredentials credentials)
    {
        ImapClient client = new ImapClient();
        await client.ConnectAsync("imap.gmail.com", 993, true);
        await client.AuthenticateAsync(credentials.Email, credentials.AppPassword);
        return client;
    }


    public static async Task<MimeMessage> FindLastMessageWithTitle(this ImapClient client, string subjectName)
    {
        IMailFolder inbox = client.Inbox;
        _ = await inbox.OpenAsync(FolderAccess.ReadOnly);

        Console.WriteLine("Total messages: {0}", inbox.Count);
        Console.WriteLine("Recent messages: {0}", inbox.Recent);

        IList<UniqueId> uids = await inbox.SearchAsync(SearchQuery.SubjectContains(subjectName));

        List<MimeMessage> messages = inbox.GetMessagesWithId(uids);

        MimeMessage messageWithMostRecentDate = messages.First(x => x.Date.Equals(messages.Max(x => x.Date)));


        await client.DisconnectAsync(true);
        return messageWithMostRecentDate;
    }
}
