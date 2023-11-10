﻿using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;

namespace ProcedureMakerServer.TemplateManagement.PdfManagement;

public static class PdfMerger
{
    public static string MergePdfs(List<string> pdfPaths)
    {
        var documents = pdfPaths.Select(p => PdfReader.Open(p, PdfDocumentOpenMode.Import)).ToList();

        using PdfDocument outPdf = new PdfDocument();

        documents.ForEach(d => outPdf.AppendDocument(d));

        string savePath = $"{Guid.NewGuid()}.pdf";
        outPdf.Save(savePath);

        documents.ForEach(x => x.Dispose());

        return savePath;
    }
}