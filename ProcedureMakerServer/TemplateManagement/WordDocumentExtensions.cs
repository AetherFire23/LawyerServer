
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ProcedureMakerServer.Constants;
using ProcedureMakerServer.TemplateManagement.PdfManagement;

namespace ProcedureMakerServer.TemplateManagement;

public static class WordDocumentExtensions
{

	public static List<Run> GetRuns(this WordprocessingDocument self)
	{
		List<Run> rs = self.MainDocumentPart.Document.Descendants<Run>().ToList();
		return rs;
	}

	public static Run FindRunWithChildInnerText(this WordprocessingDocument self, string childInnerText)
	{
		Run? run = self.GetRuns()
			.FirstOrDefault(r => r.ChildElements.Any(x => x.InnerText.Contains(childInnerText)));

		return run;
	}

	public static async Task<string> ConvertToPdf(WordDocGenerationInfo documentPath)
	{
		(string pdfSaveDirectoryPath, string outFileName) = GenerateOutPaths(documentPath.RandomlyGenerated, ".pdf");

		await DocxToPdfConverter.CreatePdf(documentPath.FilePath, pdfSaveDirectoryPath);
		return outFileName;
	}

	public static async Task<string> ConvertToHtml(WordDocGenerationInfo documentPath)
	{
		(string tempDirectoryPath, string outFileName) = GenerateOutPaths(documentPath.RandomlyGenerated, ".html");

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
		Run run = self.FindRunWithChildInnerText("bccReceivers");

		DocumentFormat.OpenXml.OpenXmlElement? paragraph = run.Parent;

		paragraph.RemoveAllChildren();

		foreach (T content in contentToFill)
		{
			_ = paragraph.InsertAfterSelf(paragraphTextToAdd(content));
		}
		paragraph.Remove();
	}

	// out can be pdf, etc.
	private static (string OutSavePath, string OutFilePath) GenerateOutPaths(string docxFileName, string outExtension)
	{
		string outFilePath = ConstantPaths.TemporaryFilesPath + docxFileName + outExtension;
		(string OutPath, string OutFileName) paths = (ConstantPaths.TemporaryFilesPath, outFilePath);
		return paths;
	}

	/// <summary>
	/// dont include the . in the extension
	/// </summary>

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