
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ProcedureMakerServer.Constants;
using ProcedureMakerServer.TemplateManagement;

namespace ProcedureMakerServer.Utils;

public static class WordHelper
{
    private const string DocumentFolderPath = "DocsTemplates/";

    public static byte[] ReadDocumentBytesAt(string wordDocumentName)
    {
        string path = DocumentFolderPath + wordDocumentName;
        if (!File.Exists(path)) throw new Exception("Template did not exist, file should match enum type value");

        var bytes = File.ReadAllBytes(path);

        return bytes;
    }

    public static WordDocInfo OpenDocumentFromBytes(byte[] bytes)
    {
        var wordInfo = new WordDocInfo();
        using (var stream = new MemoryStream(bytes, 0, (int)bytes.Length))
        {
            using var readonlyCopy = WordprocessingDocument.Open(stream, false);
            using var pack = readonlyCopy.Clone(wordInfo.FilePath);
        }


        return wordInfo;
    }

    public static ParagraphProperties GetPropertiesOrCreate(this Paragraph self)
    {
        if (self.ParagraphProperties is null || !self.ParagraphProperties.Any())
        {
            self.ParagraphProperties = new ParagraphProperties();
        }
        return self.ParagraphProperties;
    }
}
