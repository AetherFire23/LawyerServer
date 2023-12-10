using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Exceptions;
using ProcedureMakerServer.Initialization;
using ProcedureMakerServer.Scratches;

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

        //    // 2. NOTIFY
        //// document part
        //var presentationNoticePath = await DocumentMaker.GenerateDocumentAsPdf(DocumentDummies.CreateCaseDto(), DocumentTypes.PresentationNotice);
        //var backingPdfPath = await DocumentMaker.GenerateDocumentAsPdf(DocumentDummies.CreateCaseDto(), DocumentTypes.Backing, new BackingFillerParams() { DocumentName = DocumentTypes.PresentationNotice.ToString()});
        //string mergedNotificationPath = PdfMerger.MergePdfs(new() { presentationNoticePath, backingPdfPath });


        //// Send email part
        //EmailCredentials credentials = new EmailCredentials()
        //{
        //    Email = "richerf3212@gmail.com",
        //    AppPassword = MyPassword.Pass,
        //};

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









        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.ConfigureJwt();

        // removes errors where aspnet sends bad request if a nonnullable is sent 
        builder.Services.AddControllers(
                options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

        builder.Services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
        }); // omgf
        AppBuilderHelper.Configure(builder);


        var app = builder.Build();

        app.UseExceptionHandler(exceptionHandler =>
        {
            exceptionHandler.Run(async context =>
            {
                IExceptionHandlerPathFeature? exceptionHandlerFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                if (exceptionHandlerFeature?.Error is HttpExceptionBase exBase)
                {
                    context.Response.StatusCode = (int)exBase.StatusCode;

                    ExceptionResponseData responseData = new()
                    {
                        Data = exBase.Data,
                        Message = exBase.Message,
                    };

                    await context.Response.WriteAsJsonAsync(responseData);
                }
            });
        });

        await AppConfigHelper.ConfigureApp(app);

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    // only in var is cannot be  implicitly changed
    public async Task SendEmailAsync()
    {
        string senderEmail = "richerf3212@gmail.com";
        string senderName;
        string recipientEmail = "richerf3212@gmail.com";
        string subject = "test";
        string body = "<h1> test </h1>";
        string clientId;
        string clientSecret;
        string refreshToken;




        var secrets = new ClientSecrets
        {
            ClientId = Environment.GetEnvironmentVariable("GMailClientId"),
            ClientSecret = Environment.GetEnvironmentVariable("GMailClientSecret")
        };
    }

}
