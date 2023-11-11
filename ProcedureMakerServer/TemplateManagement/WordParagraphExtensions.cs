using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using ProcedureMakerServer.Utils;

namespace ProcedureMakerServer.TemplateManagement;

public static class WordParagraphExtensions
{
    public static void InsertParagraphAfterSelf(this Paragraph self, string text)
    {
        var paragraph = new Paragraph();
        var run = new Run();
        var txt = new Text(text);

        paragraph.Append(run);
        run.Append(txt);
        self.InsertAfterSelf(paragraph);
    }

    public static Paragraph CreateBlankParagraph(string text = "")
    {
        var par = new Paragraph();
        var run = new Run();
        var txt = new Text(text);
        par.AppendChild(new ParagraphProperties());
        par.AppendChild(run);
        run.AppendChild(txt);
        return par;
    }
    public static Paragraph CreateStyledParagraph(List<OpenXmlElement> openXmlElement, string text = "")
    {
        var blankParagraph = CreateBlankParagraph(text);
        var paragraphProps = blankParagraph.GetPropertiesOrCreate();
        openXmlElement.ForEach(p => paragraphProps.AppendChild(p));

        return blankParagraph;
    }
}
