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

        // 1. DOC GEN
        // Generate document to be filled
        // user interaction (signs document, fills it)
        var s = DocumentCache.GetDocumentCopy(DocumentTypes.Backing);
        var t = DocumentMaker.GenerateDocument(DocumentDummies.CaseDto, DocumentTypes.Backing);

        
        // 2. NOTIFY
        // sends the modified document back as pdf (check for digitally signing options)


        // Merge document + backing
        // create html body for notification label
        // format subject title
        // (dont forget number of pages inside the document)

        // 3. ARCHIVE NOTIFICATION
        // Read the email from the .max() MimeMessage with the researched titled
        // From said email, fill a docx version of proof of notification
        // Convert proof of notification to PDF
        // merge signed document + proof of notification + backing
        // return it to user

        // bonus : find a way not to have to flip the backing 



        // Controller interfaces:
        // IFile GenerateDocument(FileType)
        // IFile NotifyDocument() 



        using (PdfDocument one = PdfReader.Open("file1.pdf", PdfDocumentOpenMode.Import))
        using (PdfDocument two = PdfReader.Open("file2.pdf", PdfDocumentOpenMode.Import))
        using (PdfDocument outPdf = new PdfDocument())
        {
            CopyPages(one, outPdf);
            CopyPages(two, outPdf);

            outPdf.Save("file1and2.pdf");
        }

        void CopyPages(PdfDocument from, PdfDocument to)
        {
            for (int i = 0; i < from.PageCount; i++)
            {
                to.AddPage(from.Pages[i]);
            }
        }





        var stdOutBuffer = new StringBuilder();
        var result = await Cli.Wrap(@"C:\Program Files\LibreOffice\program\soffice.exe")
            .WithArguments(args =>
            {
                args
                .Add("--convert-to")
                .Add("pdf")
                .Add(@"C:\Doc1.docx")
                .Add("--outdir")
                .Add(@"C:\test.pdf");

            })
            .WithStandardOutputPipe(PipeTarget.ToStringBuilder(stdOutBuffer))
            .ExecuteAsync();

        Console.WriteLine(stdOutBuffer.ToString());


        int z = 0;


        //var doc = DocumentCache.GetDocumentCopy(DocumentTypes.Backing);

        // var r = doc.FindRunWithChildInnerText("wrong");
        int random = Random.Shared.Next(0, 100);

        // voir pour voir si la signature digitale est valide
        // va determiner si word vs pdf 
        // create real document
        var cdto = new CaseDto()
        {
            ManagerLawyer = new Lawyer()
            {
                FirstName = "Test",
            }
        };

        using (var document = DocumentMaker.GenerateDocument(cdto, DocumentTypes.Backing))
        {
            document.Clone("backinglolzida.docx");

        }

        // create email message
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("richerf3212@gmail.com"));
        email.To.Add(MailboxAddress.Parse("richerf3212@gmail.com"));
        email.Subject = "NOTIFICATION";


        email.Body = new TextPart(TextFormat.Html) { Text = $"<h1>Example HTML Message Body {random} </h1>" };

        // send email
        using var smtp = new SmtpClient();
        smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        smtp.Authenticate("richerf3212@gmail.com", $"dfdb aybg gjlm lxor");
        smtp.Send(email);
        smtp.Disconnect(true);


        // recolter la preuve de notification

        using (var client = new ImapClient())
        {
            client.Connect("imap.gmail.com", 993, true);

            client.Authenticate("richerf3212@gmail.com", "dfdb aybg gjlm lxor");

            // The Inbox folder is always available on all IMAP servers...
            IMailFolder inbox = client.Inbox;
            inbox.Open(FolderAccess.ReadOnly);

            Console.WriteLine("Total messages: {0}", inbox.Count);
            Console.WriteLine("Recent messages: {0}", inbox.Recent);


            IList<UniqueId> uids = inbox.Search(SearchQuery.SubjectContains("NOTIFICATION"));

            var messages = inbox.GetMessagesWithId(uids);

            Console.WriteLine(random);
            var messagesWithMostRecentDate = messages.First(x => x.Date.Equals(messages.Max(x => x.Date)));

            client.Disconnect(true);
        }


        // should this be
        // create notify document
        // create backing document


        // send email with pdf I guess






        int ixyz = 0;




        // var r = doc.CreateParagraphChainAtRunWithChildInnerText("changeme", new List<string> { "hello", "more", "evenmore", "evenmore", "more" });

        //var r = doc.MainDocumentPart.Document.Descendants<Run>()
        //     .FirstOrDefault(r => r.ChildElements.Any(x => x.InnerText.Contains("changeme")));
        ////r.RemoveAllChildren();

        //var parent = r.Parent;
        //var para = parent.AppendChild(new Paragraph());
        //var t1 = para.AppendChild(new Text("I am super cool"));
        //var para2 = r.AppendChild(new Paragraph());
        //var t2 = para2.AppendChild(new Text("I am super cool2"));


        //var para3 = r.AppendChild(new Paragraph());


        // search the run that has a child with x as a name.
        // keep the run, delete the child. Append PARAGRAPHS to the run. might work.
        // 



        //doc.SearchAndReplace("[changeme]", "<r>  <r>");

        //  string textBody = doc.ReadDocumentText();

        // doc.OverwriteBody
        // somewhat works but doesnt when between empty paragraphs. so wont work at all
        //var bookMark = doc.MainDocumentPart.Document.Descendants<BookmarkStart>()
        //        .FirstOrDefault(x => x.Name == "Defenders");


        //// faut jfasse une chaine de paragraphes

        //var parent = bookMark.Parent;

        //var bod= doc.MainDocumentPart.Document.Body;


        //// parent du bookmark, cest quoi
        //Paragraph para = parent.AppendChild(new Paragraph());

        //Run run = para.AppendChild(new Run());

        //run.AppendChild(new Text("blabladou"));


        //doc.Clone("backingAppended.docx");



        // var test = DocumentCache.GetDocument(DocumentTypes.Backing);


        //RTKQueryExporter.ExportQueriesToFile();
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
