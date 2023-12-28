using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MimeKit;
using MimeKit.IO;
using ProcedureShared.Dtos;
using System.Globalization;

namespace ProcedureMakerServer.TemplateManagement.DocumentFillers;

[DocumentFiller(DocumentTypes.ProofOfNotification)]
public class ProofOfNotificationFiller : DocumentFillerBase
{

	protected override void CreateFixedReplacementKeywords(CaseDto caseDto, List<(string From, string To)> keywordMap, object? additional = null)
	{
		MimeMessage? email = additional as MimeMessage;
		MimeEntity? attachment = email.Attachments.FirstOrDefault();

		// specific to proof 
		long size = MeasureAttachmentSize(attachment as MimePart);
		keywordMap.Add(("senderName", email.From.ToString()));
		keywordMap.Add(("subjectName", email.Subject));
		keywordMap.Add(("date", email.Date.ToString()));
		keywordMap.Add(("attachmentSize", size.ToString()));
		keywordMap.Add(("attachmentName", attachment.ContentType.Name));

		CultureInfo fr = new CultureInfo("fr-FR");
		keywordMap.Add(("dayMonthYear", email.Date.ToString("dd/MM/yyyy", fr)));



		//l.Add(("bcReceivers", ));

		//  l.Add(("attachment", email.Attachments.ToList().First().ToString()));
		// l.Add(("attachmentSize", email.Attachments.First().siz));

	}

	private static long MeasureAttachmentSize(MimePart part)
	{
		using (MeasuringStream measure = new MeasuringStream())
		{
			part.Content.DecodeTo(measure);
			return measure.Length;
		}
	}

	protected override void FillArrayFields(CaseDto caseDto, WordprocessingDocument document, object? additional = null)
	{
		Document document2 = document.MainDocumentPart.Document;
		MimeMessage? email = additional as MimeMessage;
		List<MailboxAddress?> bccs = email.Bcc.Select(x => x as MailboxAddress).ToList();

		Func<MailboxAddress, Paragraph> paragraphFactory = message =>
		{
			Paragraph paragraph = WordParagraphExtensions.CreateBlankParagraph(message.Address);
			return paragraph;
		};

		document.FillAnArrayField("bccReceivers", bccs, paragraphFactory);
	}
}
