using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ProcedureMakerServer.Dtos;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ProcedureMakerServer.TemplateManagement;

public abstract class DocumentFillerBase : IDocumentFiller
{
    protected DocumentTypes DocumentType;
    protected virtual List<(string From, string To)> GetStaticReplacementKeywords(CaseDto caseDto, object? additional = null)
    {
        List<(string From, string To)> pairs = new();
        return pairs;
    }

    // must be done by hand else it will 100% fuck formatting
    protected virtual void FillArrayFields(CaseDto caseDto, WordprocessingDocument document, object? additional = null)
    {

    }

    public DocumentFillerBase()
    {
        DocumentType = this.GetType().GetCustomAttribute<DocumentFillerAttribute>().DocumentType;
    }


    public virtual WordprocessingDocument GenerateDocument(CaseDto dto, DocumentTypes documentType, object? additionalParams = null)
    {
        var document = DocumentCache.GetDocumentCopy(documentType);
        var keyWords = GetStaticReplacementKeywords(dto, additionalParams);

        foreach ((string From, string To) in keyWords)
        {
            document.SearchAndReplace(From, To);
        }

        this.FillArrayFields(dto, document, additionalParams);

        return document;
    }

    public virtual string FormatEmailSubjectTitle(CaseDto dto) { return ""; }
}
