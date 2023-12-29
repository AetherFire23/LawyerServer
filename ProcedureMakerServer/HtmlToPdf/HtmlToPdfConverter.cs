using CliWrap;
using ProcedureShared.FunExtensions;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using System.Diagnostics;
using System.Text;
namespace ProcedureMakerServer.HtmlToPdf;

public class HtmlToPdfConverter
{
	// https://groups.google.com/a/chromium.org/g/chromium-dev/c/LXZQz6UpVZI
	// So I have to use a tool that can access the Chrome Dev Tools and not necessarily the CLI.
	// Puppeteer seems to do that


	// seems I can do it like this?




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


	// I can take a screenshot of it I guess to display image inside of React:
	// --screenshot=[output path]
	// 
	// https://stackoverflow.com/questions/55418415/disable-chromes-default-headers-footers-in-headless-print-to-pdf

	// this fucking worked!
	//										     | this is output path   | this is source path
	//./chrome.exe --headless --print-to-pdf=C:\Temp\htmltopdf\sex.pdf "C:\Temp\htmltopdf\test.html"
	// and the "--no-margins" parameter



	//public async Task converthtmltopdfpuppeteer(string html)
	//{
	//	using var browserFetcher = new BrowserFetcher();
	//	await browserFetcher.DownloadAsync();
	//	await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
	//	await using var page = await browser.NewPageAsync();
	//	//await page.GoToAsync("http://www.google.com"); // In case of fonts being loaded from a CDN, use WaitUntilNavigation.Networkidle0 as a second param.

	//	await page.SetContentAsync(html);
	//	await page.EvaluateExpressionHandleAsync("document.fonts.ready"); // Wait for fonts to be loaded. Omitting this might result in no text rendered in pdf.

	//	//var pdfOptions = new PdfOptions()
	//	//{
	//	//	MarginOptions = new MarginOptions
	//	//	{
	//	//		Bottom = "0",
	//	//		Top = "0",
	//	//		Left = "0",
	//	//		Right = "0",
	//	//	},
	//	//};

	//	string outPath = PathExtensions.CreateTempFilePath("pdf");

	//	try
	//	{
	//		await page.PdfAsync(outPath);

	//	}
	//	catch(Exception ex)
	//	{
	//		Console.WriteLine(ex);
	//	}

	//	Process.Start("explorer.exe", outPath);

	//	//await page.PdfAsync(outputFile);
	//}

	/// <returns> filePath to pdf </returns>
	public async Task<string> ConvertHtmlToPdf(string html)
	{
		string randomPathFromHtml = PathExtensions.CreateTempFilePath("html");
		string randomOutPathPdf = PathExtensions.CreateTempFilePath("pdf");

		var bytes = Encoding.UTF8.GetBytes(html);
		File.WriteAllBytes(randomPathFromHtml, bytes);

		await WrappedHtmlToPdf(randomPathFromHtml, randomOutPathPdf);

		return randomOutPathPdf;
	}

	public async Task WrappedHtmlToPdf(string fromPath, string outPath)
	{
		string chromeExecutablePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Google", "Chrome", "Application", "chrome.exe");

		var sb = new StringBuilder();
		var result = await Cli.Wrap(chromeExecutablePath)
			.WithArguments(args =>
			{
				args
				.Add("--headless")
				.Add($"--print-to-pdf={outPath}")
				.Add("--no-pdf-header-footer")
				.Add(fromPath);
			})
			.WithStandardOutputPipe(PipeTarget.ToStringBuilder(sb))
			.WithStandardErrorPipe(PipeTarget.ToStringBuilder(sb))
			.ExecuteAsync();

		Console.WriteLine(sb.ToString());

		Process.Start("explorer.exe", outPath);
	}
}
