﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProcedureMakerServer.Billing.StatementDtos;
using ProcedureMakerServer.Dtos;
using System.Runtime.InteropServices.Marshalling;
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
	/// <returns> Html as string </returns>
	public async Task<string> RenderInvoiceToHtml(CaseDto caseDto, InvoiceDto invoice)
	{
		var parameters = new Dictionary<string, object?>()
		{
			{ nameof(Component1.CaseDto), caseDto },
			{ nameof(Component1.InvoiceDto), invoice },
		};
		var html = await RendererServices.RenderView<Component1>(parameters);
		return html;
	}
}