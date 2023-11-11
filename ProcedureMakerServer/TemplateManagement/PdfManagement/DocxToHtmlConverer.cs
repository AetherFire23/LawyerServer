using CliWrap;
using System.Text;

namespace ProcedureMakerServer.TemplateManagement.PdfManagement;

public static class DocxToHtmlConverer
{

    public static async Task ConvertToHtml(string fromDocxPath, string htmlOutPath)
    {
        // if (!File.Exists(fromDocxPath)) throw new Exception();

        var stdOutBuffer = new StringBuilder();
        // hardcoed executable path


        // using relative pathing now for toPdfPath
        string cliPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "LibreOffice", "program", "soffice.exe"); ;
        var result = await Cli.Wrap(cliPath)
            .WithArguments(args =>
            {
                args
                .Add("--convert-to")
                .Add("html")
                .Add(fromDocxPath)
                .Add("--outdir")
                .Add(htmlOutPath);

            })
            .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
            .WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
            .ExecuteAsync();

        Console.WriteLine(stdOutBuffer.ToString());
    }
}
