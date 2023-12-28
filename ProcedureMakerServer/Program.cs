using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Exceptions;
using ProcedureMakerServer.Initialization;
using ProcedureMakerServer.Scratches;
using ProcedureMakerServer.TemplateManagement;

namespace ProcedureMakerServer;



// pour le tool de rtkQuery
// y aller peut-etre comme reinforeced typings mais jpas sur comment
// sinon nu pre-build
// sinon juste un solution thing où j'launch ma premiere solution avant
//  en tant que headless console.


public class Program
{
	public static async Task Main(string[] args)
	{
		// //// 1. DOC GEN
		//


		//    // 2. NOTIFY
		//// document part
		//var presentationNoticePath = await DocumentMaker.GenerateDocumentAsPdf(DocumentDummies.CreateCaseDto(), DocumentTypes.PresentationNotice);
		//var backingPdfPath = await DocumentMaker.GenerateDocumentAsPdf(DocumentDummies.CreateCaseDto(), DocumentTypes.Backing, new BackingFillerParams() { DocumentName = DocumentTypes.PresentationNotice.ToString()});
		//string mergedNotificationPath = PdfMerger.MergePdfs(new() { presentationNoticePath, backingPdfPath });


		// Send email part
		// EmailCredentials credentials = new EmailCredentials()
		// {
		//     Email = "richerf3212@gmail.com",
		//     AppPassword = MyPassword.Pass,
		// };

		//SendEmailInfo sendingInfo = new SendEmailInfo();
		//sendingInfo.Subject = await DocumentMaker.GenerateEmailSubject(DocumentDummies.CreateCaseDto(), DocumentTypes.PresentationNotice);
		//string htmlPath = await DocumentMaker.GenerateNotificationBorderAsHtml(DocumentDummies.CreateCaseDto(), mergedNotificationPath, DocumentTypes.PresentationNotice.ToString());
		//sendingInfo.EmailHtmlBody = File.ReadAllText(htmlPath);
		//sendingInfo.To = "richerf3212@gmail.com";
		//sendingInfo.PdfAttachmentPath = mergedNotificationPath;
		//await EmailSender.NotifyDocument(credentials, sendingInfo);


		//// 3. ARCHIVE NOTIFICATION

		//string ProofOfNotificationPath = await DocumentMaker.GenerateProofOfNotificationAsPdf(DocumentDummies.CreateCaseDto(),
		//    sendingInfo.Subject,
		//    credentials);

		//string mergedWithProof = PdfMerger.MergePdfs(new() { presentationNoticePath, ProofOfNotificationPath, backingPdfPath });

		//int ixyz = 0;


		WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


		_ = builder.Services.AddControllers()
			.AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

			});

		_ = builder.Services.AddEndpointsApiExplorer();
		_ = builder.Services.AddSwaggerGen(c =>
		{
			c.SchemaFilter<XEnumNamesSchemaFilter>();
			c.UseAllOfToExtendReferenceSchemas();
			//c.UseInlineDefinitionsForEnums();
			//    c.DescribeAllEnumsAsStrings(); // this will do the trick
		});

		builder.Services.InitializeAndRegisterDocumentFillers();

		builder.ConfigureJwt();

		// removes errors where aspnet sends bad request if a nonnullable is sent 
		_ = builder.Services.AddControllers(
				options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

		_ = builder.Services.AddRouting(options =>
		{
			options.LowercaseUrls = true;
		}); // omgf

		AppBuilderHelper.Configure(builder);


		WebApplication app = builder.Build();

		_ = app.UseExceptionHandler(exceptionHandler =>
		{
			exceptionHandler.Run(async context =>
			{
				IExceptionHandlerPathFeature? exceptionHandlerFeature = context.Features.Get<IExceptionHandlerPathFeature>();

				if (exceptionHandlerFeature?.Error is HttpExceptionBase exBase)
				{
					context.Response.StatusCode = (int)exBase.StatusCode;

					ExceptionResponseData responseData = new()
					{
						Data = exBase.ExceptionData,
						Message = exBase.HttpMessage,
					};

					await context.Response.WriteAsJsonAsync(responseData);
				}
			});
		});

		await AppConfigHelper.ConfigureApp(app);

		if (app.Environment.IsDevelopment())
		{
			_ = app.UseSwagger();
			_ = app.UseSwaggerUI();
		}

		_ = app.UseHttpsRedirection();

		_ = app.UseAuthorization();

		_ = app.MapControllers();

		app.Run();
	}

	// only in var is cannot be  implicitly changed
	public async Task SendEmailAsync()
	{
		_ = new ClientSecrets
		{
			ClientId = Environment.GetEnvironmentVariable("GMailClientId"),
			ClientSecret = Environment.GetEnvironmentVariable("GMailClientSecret")
		};
	}
}
