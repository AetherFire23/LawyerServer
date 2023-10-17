using Newtonsoft.Json;
using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Initialization;
namespace ProcedureMakerServer;

// case = dossier avocat
// affair = de cour
// file = template
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
