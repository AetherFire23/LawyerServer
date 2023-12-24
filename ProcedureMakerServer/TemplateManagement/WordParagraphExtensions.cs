using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using ProcedureMakerServer.Utils;

namespace ProcedureMakerServer.TemplateManagement;

public static class WordParagraphExtensions
{
    public static void InsertParagraphAfterSelf(this Paragraph self, string text)
    {
        Paragraph paragraph = new Paragraph();
        Run run = new Run();
        Text txt = new Text(text);

        paragraph.Append(run);
        run.Append(txt);
        _ = self.InsertAfterSelf(paragraph);
    }

    public static Paragraph CreateBlankParagraph(string text = "")
    {
        Paragraph par = new Paragraph();
        Run run = new Run();
        Text txt = new Text(text);
        _ = par.AppendChild(new ParagraphProperties());
        _ = par.AppendChild(run);
        _ = run.AppendChild(txt);
        return par;
    }
    public static Paragraph CreateStyledParagraph(List<OpenXmlElement> openXmlElement, string text = "")
    {
        Paragraph blankParagraph = CreateBlankParagraph(text);
        ParagraphProperties paragraphProps = blankParagraph.GetPropertiesOrCreate();
        openXmlElement.ForEach(p => paragraphProps.AppendChild(p));

        return blankParagraph;
    }
}
