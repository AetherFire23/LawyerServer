using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MailKit.Net.Imap;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MimeKit;
using MimeKit.Text;
using Newtonsoft.Json;
using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Dtos;
using ProcedureMakerServer.Entities;
using ProcedureMakerServer.Exceptions;
using ProcedureMakerServer.Initialization;
using ProcedureMakerServer.Scratches;
using ProcedureMakerServer.TemplateManagement;
using ProcedureMakerServer.Utils;
using MailKit.Search;
using CliWrap;
using System.Text;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;
using ProcedureMakerServer.Services;
using ProcedureMakerServer.TemplateManagement.PdfManagement;
using ProcedureMakerServer.EmailMaker;
using ProcedureMakerServer.TemplateManagement.DocumentFillers;

namespace ProcedureMakerServer;

// case = dossier avocat
// affair = de cour
// file = template


// pour reinforced typings <<target compile typescript was not found >> 
//  <Target Name="CompileTypeScript" />



// setter une configuremethod
//<RtConfigurationMethod>ProcedureMakerServer.ReinforcedTypingsConfiguration.Configure</RtConfigurationMethod>
// pas oublier : faire matcher les namespaces


// le catual target
// 		<RtTargetDirectory>..\..\lawyer-procedure\mercichatgpt</RtTargetDirectory>

// pour pas toute crisser dans un seul file et pouvoir import les fichiers 1 a un (require la configure method()
//{
//    builder.Global(config => config.CamelCaseForProperties()
//        .AutoOptionalProperties()
//        .UseModules());


// redis
// stringpool




//app.UseExceptionHandler(
//              options =>
//              {
//                  options.Run(
//                      async context =>
//                      {
//                          context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
//                          context.Response.ContentType = "text/html";
//                          var ex = context.Features.Get<IExceptionHandlerFeature>();
//                          if (ex != null)
//                          {
//                              var err = $"<h1>Error: {ex.Error.Message}</h1>{ex.Error.StackTrace}";
//                              await context.Response.WriteAsync(err).ConfigureAwait(false);
//                          }
//                      });
//              }
//          );



// spaceing between lines?
// structure is body -> paragraph => run -> text

//var text = new Text("dwdw");

//var run = new Run(text);

//var par = new Paragraph(run);

//var space = new SpacingBetweenLines();
//space.After = "0";
//par.AppendChild(space);

//doc.MainDocumentPart.Document.Body.AppendChild(par);

public class Program
{
    public static async Task Main(string[] args)
    {

        //var doc2 = WordprocessingDocument.Open(@"DocsTemplates\ProofOfNotification.docx", true);

        //var run = doc2.FindRunWithChildInnerText("bccReceivers");

       
        // steps :
        // class RuntimeInformation to check on which platofrm I am
        // another class also exists to check if i am on x86
        // 

        // 1. DOC GEN
        // Generate document to be filled
        // user interaction (signs document, fills it)

        //return some docx here here

        var doc = DocumentMaker.GenerateDocument(DocumentDummies.CaseDto, DocumentTypes.Backing);




        // 2. NOTIFY
        // sends the modified document back as pdf (check for digitally signing options)
        // IFOrmFile.CreatFileTo(.pdfShit)
        var presentationNoticePath = await DocumentMaker.GenerateDocumentAsPdf(DocumentDummies.CaseDto, DocumentTypes.PresentationNotice);

        // Create Backing as PDF
        var backingPdfPath = await DocumentMaker.GenerateDocumentAsPdf(DocumentDummies.CaseDto, DocumentTypes.Backing);


        // this needs to be an attachment
        // Merge signed document + backing
        string mergedPdfPath = PdfMerger.MergePdfs(new() { presentationNoticePath, backingPdfPath });


        // create html body for notification label

        // format subject title

        EmailCredentials credentials = new EmailCredentials();
        credentials.Email = "richerf3212@gmail.com";
        credentials.AppPassword = MyPassword.Pass;


        SendEmailInfo sendingInfo = new SendEmailInfo();
        sendingInfo.Subject = await DocumentMaker.GenerateEmailSubject(DocumentDummies.CaseDto, DocumentTypes.Backing);
        string htmlPath = await DocumentMaker.GenerateNotificationBorderAsHtml(DocumentDummies.CaseDto, presentationNoticePath);
        sendingInfo.EmailHtmlBody = File.ReadAllText(htmlPath);
        sendingInfo.To = "richerf3212@gmail.com";
        sendingInfo.PdfAttachmentPath = mergedPdfPath;
        await EmailSender.NotifyDocument(credentials, sendingInfo);

        // (dont forget number of pages inside the document)
        // 3. ARCHIVE NOTIFICATION
        // Read the email from the .max() MimeMessage with the researched titled


        string ProofOfNotificationPath = await DocumentMaker.GenerateProofOfNotificationAsPdf(DocumentDummies.CaseDto,
            sendingInfo.Subject,
            credentials);

        string mergedPath = PdfMerger.MergePdfs(new() { presentationNoticePath, ProofOfNotificationPath, backingPdfPath });

        // return it to user
        // Dunno what thype it should be haha

        // bonus : find a way not to have to flip the backing 





        int ixyz = 0;



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
                IExceptionHandlerPathFeature exceptionHandlerFeature = context.Features.Get<IExceptionHandlerPathFeature>();

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


}
