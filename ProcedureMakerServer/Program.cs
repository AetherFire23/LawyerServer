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
using System.Text.RegularExpressions;
using Org.BouncyCastle.Utilities;

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
         //// 1. DOC GEN

            // 2. NOTIFY
        // document part
        var presentationNoticePath = await DocumentMaker.GenerateDocumentAsPdf(DocumentDummies.CreateCaseDto(), DocumentTypes.PresentationNotice);
        var backingPdfPath = await DocumentMaker.GenerateDocumentAsPdf(DocumentDummies.CreateCaseDto(), DocumentTypes.Backing, new BackingFillerParams() { DocumentName = DocumentTypes.PresentationNotice.ToString()});
        string mergedNotificationPath = PdfMerger.MergePdfs(new() { presentationNoticePath, backingPdfPath });


        // Send email part
        EmailCredentials credentials = new EmailCredentials()
        {
            Email = "richerf3212@gmail.com",
            AppPassword = MyPassword.Pass,
        };

        SendEmailInfo sendingInfo = new SendEmailInfo();
        sendingInfo.Subject = await DocumentMaker.GenerateEmailSubject(DocumentDummies.CreateCaseDto(), DocumentTypes.PresentationNotice);
        string htmlPath = await DocumentMaker.GenerateNotificationBorderAsHtml(DocumentDummies.CreateCaseDto(), mergedNotificationPath, DocumentTypes.PresentationNotice.ToString());
        sendingInfo.EmailHtmlBody = File.ReadAllText(htmlPath);
        sendingInfo.To = "richerf3212@gmail.com";
        sendingInfo.PdfAttachmentPath = mergedNotificationPath;
        await EmailSender.NotifyDocument(credentials, sendingInfo);


        // 3. ARCHIVE NOTIFICATION

        string ProofOfNotificationPath = await DocumentMaker.GenerateProofOfNotificationAsPdf(DocumentDummies.CreateCaseDto(),
            sendingInfo.Subject,
            credentials);

        string mergedWithProof = PdfMerger.MergePdfs(new() { presentationNoticePath, ProofOfNotificationPath, backingPdfPath });

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
