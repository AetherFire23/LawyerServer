using MimeKit;

namespace ProcedureMakerServer.Extensions;

public static class MimeMessageExtensions
{
	public static void AddTos(this MimeMessage message, List<string> tos)
	{
		// could validate by using the TryParse()
		foreach (var to in tos)
		{
			message.To.Add(MailboxAddress.Parse(to));
		}
	}
	public static void AddCc(this MimeMessage message, List<string> ccs)
	{
		// could validate by using the TryParse()
		foreach (var cc in ccs)
		{
			message.Cc.Add(MailboxAddress.Parse(cc));
		}
	}

	public static void AddBcc(this MimeMessage message, List<string> bccs)
	{
		// could validate by using the TryParse()
		foreach (var bcc in bccs)
		{
			message.Bcc.Add(MailboxAddress.Parse(bcc));
		}
	}
}
