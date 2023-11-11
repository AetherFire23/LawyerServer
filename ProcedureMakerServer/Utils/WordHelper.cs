using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ProcedureMakerServer.Constants;

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

    public static WordprocessingDocument OpenDocumentFromBytes(byte[] bytes)
    {
        var stream = new MemoryStream(bytes, 0, (int)bytes.Length);
        var readonlyCopy = WordprocessingDocument.Open(stream, false);

        string path = Path.Combine(ConstantPaths.TemporaryFilesPath, $"{Guid.NewGuid()}.docx");
        var pack = readonlyCopy.Clone(path);

        readonlyCopy.Dispose();
        pack.Dispose();

        var writeable = WordprocessingDocument.Open(path, true);

        return writeable;
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
