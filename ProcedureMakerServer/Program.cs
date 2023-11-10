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
        var signedDocumentPath = await DocumentMaker.GenerateDocumentAsPdf(DocumentDummies.CaseDto, DocumentTypes.Backing);

        // Create Backing as PDF
        var backingPdfPath = await DocumentMaker.GenerateDocumentAsPdf(DocumentDummies.CaseDto, DocumentTypes.Backing);


        // this needs to be an attachment
        // Merge signed document + backing
        PdfMerger.MergePdfs(new() { signedDocumentPath, backingPdfPath });


        // create html body for notification label

        // format subject title

        EmailCredentials credentials = new EmailCredentials();
        credentials.Email = "richerf3212@gmail.com";
        credentials.AppPassword = MyPassword.Pass;


        SendEmailInfo sendingInfo = new SendEmailInfo();
        sendingInfo.Subject = await DocumentMaker.GenerateEmailSubject(DocumentDummies.CaseDto, DocumentTypes.Backing);
        string htmlPath = await DocumentMaker.GenerateDocumentAsHtml(DocumentDummies.CaseDto, DocumentTypes.Backing);
        sendingInfo.EmailHtmlBody = File.ReadAllText(htmlPath);
        sendingInfo.To = "richerf3212@gmail.com";

        await EmailSender.NotifyDocument(credentials, sendingInfo);

        // (dont forget number of pages inside the document)
        // 3. ARCHIVE NOTIFICATION
        // Read the email from the .max() MimeMessage with the researched titled
        ImapClient client = await EmailReceiver.GetClient(credentials);
        MimeMessage sentMessage = await EmailReceiver.FindLastMessageWithTitle(client, sendingInfo.Subject);

        // From said email, fill a docx version of proof of notification

        // mmh, proof of notification requires optional params.
        // Because I need the amount of pages.
        // maybe I should jsut make another method for that
        string ProofOfNotificationPath = await DocumentMaker.GenerateDocumentAsPdf(DocumentDummies.CaseDto, DocumentTypes.Backing);

        // merge signed document + proof of notification + backing
        // 
        string mergedPath = PdfMerger.MergePdfs(new() { signedDocumentPath, ProofOfNotificationPath, backingPdfPath });

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


        // APP CONFIG


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

        //app.UseExceptionHandler((exceptionHandler) =>
        //{
        //    exceptionHandler.Run(async context =>
        //    {
        //        var handler = context.Features.Get<IExceptionHandlerFeature>();

        //        if (handler?.Error is MyInvalidExceptionBase exception)
        //        {
        //            context.Response.StatusCode = exception.StatusCode;

        //            await context.Response.WriteAsJsonAsync(exception.Data);
        //        }
        //    });
        //});

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
