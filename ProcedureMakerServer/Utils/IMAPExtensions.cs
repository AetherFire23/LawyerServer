using MailKit;
using MimeKit;

namespace ProcedureMakerServer.Utils;

public static class IMAPExtensions
{
	public static List<MimeMessage> GetMessagesWithId(this IMailFolder self, IList<UniqueId> ids)
	{
		IEnumerable<MimeMessage> messages = ids.Select(x => self.GetMessage(x));
		return messages.ToList();
	}
}
