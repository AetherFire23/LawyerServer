using CliWrap;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ProcedureMakerServer.HtmlToPdf;

public class HtmlToPdf
{
	// chrome path: C:\Program Files\Google\Chrome\Application
	// chrome = chrome --headless --print-to-pdf="d:\\{{path and file name}}.pdf" https://google.com
	// docs:https://chromedevtools.github.io/devtools-protocol/tot/Page/#method-printToPDF
	// Guess I can 

	// https://stackoverflow.com/questions/72018079/why-cant-chromes-print-to-pdf-powershell-command-generate-a-pdf-to-some-fold
	// ./chrome.exe --headless --print-to-pdf="C:\Temp\htmltopdf" "C:\Temp\htmltopdf\test.html"


	// https://developer.chrome.com/blog/headless-chrome/
	// https://developer.chrome.com/docs/chromium/new-headless
	// https://source.chromium.org/chromium/chromium/src/+/main:components/headless/command_handler/headless_command_switches.cc?q=kPrintToPDF&ss=chromium

	// for handling the --print-to-pdf switch
	// https://source.chromium.org/chromium/chromium/src/+/main:components/headless/command_handler/headless_command_handler.cc
	// cd "C:\Program Files\google\Chrome\Application"

	// this fucking worked!
	//										     | this is output path   | this is source path
      //./chrome.exe --headless --print-to-pdf=C:\Temp\htmltopdf\sex.pdf "C:\Temp\htmltopdf\test.html"
	public async Task<string> ConvertToPdf(string html)
	{
		
		//string cliPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "LibreOffice", "program", "soffice.exe");
		//CommandResult result = await Cli.Wrap(cliPath)
		//	.WithArguments(args =>
		//	{
		//		_ = args
		//		.Add("--convert-to")
		//		.Add("pdf")
		//		.Add(fromDocxPath)
		//		.Add("--outdir")
		//		.Add(toPdfPath);
		//	})
		//	.WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
		//	.WithStandardErrorPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
		//	.ExecuteAsync();

		return string.Empty;
	}

}
