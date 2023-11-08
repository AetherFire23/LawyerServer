namespace ProcedureMakerServer.TemplateManagement;

[AttributeUsage(AttributeTargets.Class)]
public class DocumentFillerAttribute : Attribute
{
    public readonly DocumentTypes DocumentType;

    public DocumentFillerAttribute(DocumentTypes documentType)
    {
        DocumentType = documentType;
    }
}
