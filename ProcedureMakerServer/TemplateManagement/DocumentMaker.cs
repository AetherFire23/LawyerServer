using DocumentFormat.OpenXml.Packaging;
using ProcedureMakerServer.Dtos;
using System.Reflection;

namespace ProcedureMakerServer.TemplateManagement;

public static class DocumentMaker
{
    private static Dictionary<DocumentTypes, DocumentFillerBase> _documentsMap
        = InitializeDocumentFillers();
    
    private static Dictionary<DocumentTypes, DocumentFillerBase> InitializeDocumentFillers()
    {
        var documentFillers = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => CustomAttributeExtensions.GetCustomAttribute<DocumentFillerAttribute>(x) is not null
                && x.IsClass && !x.IsAbstract)
            .Select(x=> (Activator.CreateInstance(x)) as DocumentFillerBase).ToList();


        var map = new Dictionary<DocumentTypes, DocumentFillerBase>();
        foreach (var documentFiller in documentFillers)
        {
            map.Add(documentFiller.GetType().GetCustomAttribute<DocumentFillerAttribute>().DocumentType, documentFiller);
        }

        return map;
    }

    public static WordprocessingDocument GenerateDocument(CaseDto dto, DocumentTypes documentType)
    {
        var documentFiller = _documentsMap[documentType];
        WordprocessingDocument doc = documentFiller.GenerateDocument(dto);
        return doc;
    }
}
