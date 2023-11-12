using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Utils;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace ProcedureMakerServer.TemplateManagement;

public abstract class DocumentFillerBase : IDocumentFiller
{
    protected DocumentTypes DocumentType;

    // accepts a list that must be filled with the keywords that must be replaced and the word that will replace it
    protected virtual void CreateFixedReplacementKeywords(CaseDto caseDto, List<(string From, string To)> keywordMap, object? additional = null)
    {
    }

    // must be done by hand else it will 100% fuck formatting
    protected virtual void FillArrayFields(CaseDto caseDto, WordprocessingDocument document, object? additional = null)
    {

    }

    public DocumentFillerBase()
    {
        DocumentType = this.GetType().GetCustomAttribute<DocumentFillerAttribute>().DocumentType;
    }

    public virtual WordDocInfo FillDocument(CaseDto dto, DocumentTypes documentType, object? additionalParams = null)
    {
        WordDocInfo docINfo = DocumentCache.GetDocumentCopy(documentType);
        var keywords = new List<(string From, string To)>();

        CreateFixedReplacementKeywords(dto, keywords, additionalParams);
        SearchAndReplace(docINfo.FilePath, keywords);

        return docINfo;
    }

    public static void SearchAndReplace(string document, List<(string From, string To)> keywords)
    {
        // besoin d'avoir le using et le fait de pas etre dans le using ca foque toute haha
        using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(document, true))
        {
            string docText = null;
            using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
            {
                docText = sr.ReadToEnd();
            }

            foreach ((string From, string To) in keywords)
            {
                Regex regexText = new Regex(From);
                docText = regexText.Replace(docText, To);
            }

            using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
            {
                sw.Write(docText);
            }
        }
    }

    public virtual string FormatEmailSubjectTitle(CaseDto dto) { return ""; }
}
