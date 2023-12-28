namespace ProcedureMakerServer.EmailMaker;

public class SendEmailInfo
{

	public List<string> Tos { get; set; } = new();
	public List<string> Cc { get; set; } = new();
	public List<string> Bccs { get; set; } = new();

	public string Subject { get; set; } = string.Empty;
	public string EmailHtmlBody { get; set; } = string.Empty;
	public string PdfFileAttachmentPath { get; set; } = string.Empty;
}
