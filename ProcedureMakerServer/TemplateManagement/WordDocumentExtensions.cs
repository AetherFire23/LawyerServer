
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ProcedureMakerServer.Constants;
using ProcedureMakerServer.Services;
using ProcedureMakerServer.TemplateManagement.PdfManagement;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace ProcedureMakerServer.TemplateManagement;

public static class WordDocumentExtensions
{

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
    public static async Task<string> ConvertToPdf(WordDocInfo documentPath)
    {
        (string pdfSaveDirectoryPath, string outFileName) = GenerateOutPaths(documentPath.FileName, ".pdf");

        await DocxToPdfConverter.CreatePdf(documentPath.FilePath, pdfSaveDirectoryPath);
        return outFileName;
    }

    public static async Task<string> ConvertToHtml(WordDocInfo documentPath)
    {
        (string tempDirectoryPath, string outFileName) = GenerateOutPaths(documentPath.FileName, ".html");

        await DocxToHtmlConverer.ConvertToHtml(documentPath.FilePath, tempDirectoryPath);
        return outFileName;
    }

    public static string CloneDocumentAt(string fromPath, string toPath)
    {
        using (WordprocessingDocument document = WordprocessingDocument.Open(fromPath, true))
        {
            using OpenXmlPackage package = document.Clone(toPath, true);
        }
        return toPath;
    }

    public static string GetInnerXml(this WordprocessingDocument self)
    {
        return self.MainDocumentPart.Document.InnerXml;
    }

    public static void FillAnArrayField<T>(this WordprocessingDocument self, string markerText, List<T> contentToFill, Func<T, Paragraph> paragraphTextToAdd)
    {
        var run = self.FindRunWithChildInnerText("bccReceivers");

        var paragraph = run.Parent;

        paragraph.RemoveAllChildren();

        foreach (T content in contentToFill)
        {
            paragraph.InsertAfterSelf(paragraphTextToAdd(content));
        }
        paragraph.Remove();
    }

    private static (string OutSavePath, string OutFilePath) GenerateOutPaths(string docxFileName, string outExtension)
    {
        string outDirectory = ConstantPaths.TemporaryFilesPath;
        string outFilePath = outDirectory + docxFileName + outExtension;
        (string OutPath, string OutFileName) paths = (outDirectory, outFilePath);
        return paths;
    }

    private static (string FromPath, string OutSavePath, string OutFilePath) GenerateTemporaryPaths(string fromExtension, string outExtension)
    {
        string fileName = Guid.NewGuid().ToString();
        string fromPath = Path.Combine(ConstantPaths.TemporaryFilesPath, fileName + fromExtension);
        string outDirectory = ConstantPaths.TemporaryFilesPath;
        string outFilePath = outDirectory + fileName + outExtension;
        (string FromPath, string OutPath, string OutFileName) paths = (fromPath, outDirectory, outFilePath);
        return paths;
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