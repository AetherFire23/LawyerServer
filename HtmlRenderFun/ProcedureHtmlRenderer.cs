using HtmlRenderFun.Components.InvoiceComponents;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProcedureMakerServer.Billing;
using System.Diagnostics;
using System.Text;
namespace HtmlRenderFun;


public static class RendererServices
{
	// https://learn.microsoft.com/en-us/aspnet/core/blazor/components/render-components-outside-of-aspnetcore?view=aspnetcore-8.0
	private static IServiceProvider Services = InitializeServiceCollection();

	private static IServiceProvider InitializeServiceCollection()
	{
		IServiceCollection serviceCollection = new ServiceCollection();
		serviceCollection.AddLogging();
		IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

		return serviceProvider;
	}

	public static async Task<string> RenderView<T>(Dictionary<string, object?> renderParams) where T : IComponent
	{
		var loggerFactory = Services.GetRequiredService<ILoggerFactory>();
		await using var htmlRenderer = new HtmlRenderer(Services, loggerFactory);

		var html = await htmlRenderer.Dispatcher.InvokeAsync(async () =>
		{
			var parameters = ParameterView.FromDictionary(renderParams);
			var output = await htmlRenderer.RenderComponentAsync<T>(parameters);

			return output.ToHtmlString();
		});

		return html;
	}
}

public class ProcedureHtmlRenderer // technically teh renderer could be in another project hehe 
{
	public async Task<string> RenderInvoiceToHtml(InvoiceSummary invoiceSummary)
	{
		var parameters = new Dictionary<string, object?>()
		{
			{ nameof(InvoicePage.InvoiceSummary), invoiceSummary },
		};

		var html = await RendererServices.RenderView<InvoicePage>(parameters);
		return html;
	}
}

public static class StringExtensions
{
	public static void SaveAndLaunch(this string obj)
	{
		var bytes = Encoding.UTF8.GetBytes(obj);
		string path = $"{Guid.NewGuid()}.html";
		File.WriteAllBytes(path, bytes);
		Process.Start("explorer.exe", path);
	}
}
