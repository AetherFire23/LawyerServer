using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Initialization;
using ProcedureMakerServer.Scratches;

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


public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.ConfigureJwt();
        AppBuilderHelper.Configure(builder);


        // APP CONFIG


        var app = builder.Build();



        app.UseExceptionHandler((exceptionHandler) =>
        {
            exceptionHandler.Run(async context =>
            {
                var handler = context.Features.Get<IExceptionHandlerFeature>();

                if (handler?.Error is MyInvalidExceptionBase exception)
                {
                    context.Response.StatusCode = exception.StatusCode;

                    await context.Response.WriteAsJsonAsync(exception.Data);
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
