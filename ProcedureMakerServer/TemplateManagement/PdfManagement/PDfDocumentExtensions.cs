using PdfSharp.Pdf;
using System.Runtime.CompilerServices;

namespace ProcedureMakerServer.TemplateManagement.PdfManagement;

public static class PDfDocumentExtensions
{
    public static void AppendDocument(this PdfDocument self, PdfDocument other)
    {
        foreach (PdfPage page in other.Pages)
        {
            self.AddPage(page);
        }
    }
}
