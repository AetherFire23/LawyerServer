using CliWrap;
using System.Text;

namespace ProcedureMakerServer.TemplateManagement.PdfManagement;

public static class DocxToPdfConverter
{

    // default to relative path if C: path
    public static async Task CreatePdf(string fromDocxPath, string toPdfPath)
    {
       // if (!File.Exists(fromDocxPath)) throw new Exception();

        var stdOutBuffer = new StringBuilder();
        // hardcoed executable path

        
        // using relative pathing now for toPdfPath
        string cliPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "LibreOffice", "program", "soffice.exe"); 
        var result = await Cli.Wrap(cliPath)
            .WithArguments(args =>
            {
                args
                .Add("--convert-to")
                .Add("pdf")
                .Add(fromDocxPath)
                .Add("--outdir")
                .Add(toPdfPath);

            })
            .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
            .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
            .ExecuteAsync();

        Console.WriteLine(stdOutBuffer);
    }
}
