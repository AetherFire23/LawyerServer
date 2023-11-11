using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MailKit;
using Microsoft.OpenApi.Extensions;
using MimeKit;
using MimeKit.IO;
using ProcedureMakerServer.Dtos;
using System.Globalization;
using System.Reflection.Metadata;

namespace ProcedureMakerServer.TemplateManagement.DocumentFillers;

[DocumentFiller(DocumentTypes.ProofOfNotification)]
public class ProofOfNotificationFiller : DocumentFillerBase
{

    protected override void GetStaticReplacementKeywords(CaseDto caseDto, List<(string From, string To)> keywordMap, object? additional = null)
    {
        var email = additional as MimeMessage;
        var attachment = email.Attachments.FirstOrDefault();

        var l = new List<(string From, string To)>();


        // specific to proof 
        long size = MeasureAttachmentSize(attachment as MimePart);
        l.Add(("senderName", email.From.ToString()));
        l.Add(("subjectName", email.Subject));
        l.Add(("date", email.Date.ToString()));
        l.Add(("attachmentSize", size.ToString()));
        l.Add(("attachmentName", attachment.ContentType.Name));
        CultureInfo fr = new CultureInfo("fr-FR");
        l.Add(("dayMonthYear", email.Date.ToString("dd/MM/yyyy", fr)));



        //l.Add(("bcReceivers", ));

        //  l.Add(("attachment", email.Attachments.ToList().First().ToString()));
        // l.Add(("attachmentSize", email.Attachments.First().siz));

        return l;
    }


    static long MeasureAttachmentSize(MimePart part)
    {
        using (var measure = new MeasuringStream())
        {
            part.Content.DecodeTo(measure);
            return measure.Length;
        }
    }

    protected override void FillArrayFields(CaseDto caseDto, WordprocessingDocument document, object? additional = null)
    {
        var document2 = document.MainDocumentPart.Document;
        var email = additional as MimeMessage;
        var bccs = email.Bcc.Select(x => x as MailboxAddress).ToList();

        Func<MailboxAddress, Paragraph> paragraphFactory = message =>
        {
            var paragraph = WordParagraphExtensions.CreateBlankParagraph(message.Address);
            return paragraph;
        };

        document.FillAnArrayField("bccReceivers", bccs, paragraphFactory);
        int x = 0;
    }
}
