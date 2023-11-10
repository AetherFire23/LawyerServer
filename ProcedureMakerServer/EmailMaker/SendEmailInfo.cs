namespace ProcedureMakerServer.EmailMaker;

public class SendEmailInfo
{
    public string To { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string EmailHtmlBody { get; set; } = string.Empty;
}
