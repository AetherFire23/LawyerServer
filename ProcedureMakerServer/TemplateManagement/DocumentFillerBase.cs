using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ProcedureMakerServer.Dtos;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ProcedureMakerServer.TemplateManagement;

public abstract class DocumentFillerBase : IDocumentFiller
{
    protected DocumentTypes DocumentType;
    protected virtual List<(string From, string To)> GetStaticReplacementKeywords(CaseDto caseDto)
    {
        List<(string From, string To)> pairs = new();
        return pairs;
    }

    // must be done by hand else it will 100% fuck formatting
    protected virtual void FillArrayFields(CaseDto caseDto, WordprocessingDocument document)
    {

    }

    public DocumentFillerBase()
    {
        DocumentType = this.GetType().GetCustomAttribute<DocumentFillerAttribute>().DocumentType;
    }


    public virtual WordprocessingDocument GenerateDocument(CaseDto dto)
    {
        var document = DocumentCache.GetDocumentCopy(DocumentTypes.Backing);
        var keyWords = GetStaticReplacementKeywords(dto);

        foreach ((string From, string To) in keyWords)
        {
            document.SearchAndReplace(From, To);
        }

        this.FillArrayFields(dto, document);

        return document;
    }

    public abstract string FormatEmailSubjectTitle(CaseDto dto);
}
