
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.RegularExpressions;

namespace ProcedureMakerServer.TemplateManagement;

public static class WordDocumentExtensions
{
    public static void SearchAndReplace(this WordprocessingDocument self, string from, string to)
    {
        string docText;
        using (StreamReader sr = new StreamReader(self.MainDocumentPart.GetStream()))
        {
            docText = sr.ReadToEnd();
        }

        Regex regexText = new Regex($"{from}");
        docText = regexText.Replace(docText, $"{to}");

        using (StreamWriter sw = new StreamWriter(self.MainDocumentPart.GetStream(FileMode.Create)))
        {
            sw.Write(docText);
        }
    }

    public static (Paragraph, Text) CreateParagraphWithText(this Run self, string text)
    {
        SpacingBetweenLines spacing = new SpacingBetweenLines() { Line = "240", LineRule = LineSpacingRuleValues.Exact, Before = "0", After = "0" };
        var para = self.AppendChild(new Paragraph(spacing));
        var t = para.AppendChild(new Text(text));

        return (para, t);
    }

    public static List<Run> GetRuns(this WordprocessingDocument self)
    {
        var rs = self.MainDocumentPart.Document.Descendants<Run>().ToList();
        return rs;
    }

    public static Run FindRunWithChildInnerText(this WordprocessingDocument self, string childInnerText)
    {
        var run = self.GetRuns()
            .FirstOrDefault(r => r.ChildElements.Any(x => x.InnerText.Contains(childInnerText)));

        return run;
    }

    public static void ReplaceWords(this WordprocessingDocument self, List<(string From, string To)> wordMaps)
    {
        foreach ((string From, string To) in wordMaps)
        {
            self.SearchAndReplace(From, To);
        }
    }

    public static string ReadDocumentText(this WordprocessingDocument self)
    {
        string read;
        using (var reader = new StreamReader(self.MainDocumentPart.GetStream()))
        {
            read = reader.ReadToEnd();
        }
        return read;
    }

    public static void OverwriteBody(this WordprocessingDocument self, string overwrittenBody)
    {
        using (var writer = new StreamWriter(self.MainDocumentPart.GetStream()))
        {
            writer.Write(overwrittenBody);
        }
    }
}


//// https://stackoverflow.com/questions/4832131/how-do-i-insert-an-element-in-a-docx-file-at-a-specific-location-using-the-open
//public static void AddTextAfterBookmark(this WordprocessingDocument self, string bookmarkName, string text)
//{
//    var bookMark = self.MainDocumentPart.Document.Descendants<BookmarkStart>()
//            .FirstOrDefault(x => x.Name == bookmarkName);
//    var parent = bookMark.Parent;
//    Paragraph para = parent.AppendChild(new Paragraph());
//    Run run = para.AppendChild(new Run());
//    run.AppendChild(new Text(text));

//    parent.InsertAfterSelf(para);
//}