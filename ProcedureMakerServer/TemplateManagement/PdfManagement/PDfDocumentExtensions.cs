using PdfSharp.Pdf;

namespace ProcedureMakerServer.TemplateManagement.PdfManagement;

public static class PDfDocumentExtensions
{
	public static void AppendDocument(this PdfDocument self, PdfDocument other)
	{
		foreach (PdfPage page in other.Pages)
		{
			_ = self.AddPage(page);
		}
	}

}
