using ProcedureMakerServer.Constants;
using ProcedureMakerServer.Utils;

namespace ProcedureMakerServer.TemplateManagement;

public static class DocumentCache
{
    private static Dictionary<DocumentTypes, byte[]> _templates = InitializeDocuments();

    private static Dictionary<DocumentTypes, byte[]> InitializeDocuments()
    {
        var templatesMap = new Dictionary<DocumentTypes, byte[]>();

        foreach (var documentType in Enum.GetValues<DocumentTypes>())
        {
            string formattedDocumentPath = $"{documentType}.docx";

            if (!File.Exists(Path.Combine(ConstantPaths.DocumentsPath, formattedDocumentPath)))
            {
                throw new Exception($"document type should exist: {formattedDocumentPath}");
            }

            templatesMap.Add(documentType, WordHelper.ReadDocumentBytesAt(formattedDocumentPath));
        }

        return templatesMap;
    }

    /// <summary> Must be disposed :) </summary>
    public static WordDocInfo GetDocumentCopy(DocumentTypes docType)
    {
        byte[] documentBytes = _templates[docType];
        byte[] buffer = new byte[documentBytes.Length];
        Array.Copy(documentBytes, buffer, documentBytes.Length);
        WordDocInfo mainDoc = WordHelper.OpenDocumentFromBytes(documentBytes);
        return mainDoc;
    }
}
