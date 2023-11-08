using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ProcedureMakerServer.Utils;
using System.Collections.Generic;

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
            templatesMap.Add(documentType, WordHelper.ReadDocumentBytesAt(formattedDocumentPath));
        }

        return templatesMap;
    }

    /// <summary> Must be disposed :) </summary>
    public static WordprocessingDocument GetDocumentCopy(DocumentTypes docType)
    {
        byte[] documentBytes = _templates[docType];

        var mainDoc = WordHelper.OpenDocumentFromBytes(documentBytes);
        return mainDoc;
    }
}
