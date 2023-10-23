using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Authentication.Interfaces;
using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Repository;
using ProcedureMakerServer.Services;
using System.Reflection;
namespace ProcedureMakerServer.Initialization;

public static class AppBuilderHelper
{
    private static WebApplicationBuilder _builder;
    public static void Configure(WebApplicationBuilder builder)
    {
        _builder = builder;
        ConfigureNpgsql(builder);
        ConfigureServices(builder);
        ConfigureAutoMapper(builder);
        ConfigureHTTPLogging(builder);
    }



    private static void ConfigureAutoMapper(WebApplicationBuilder builder)
    {
        builder.Services.AddAutoMapper(x =>
        {
            x.AddMaps(Assembly.GetExecutingAssembly());
        });
    }

    public static void ConfigureNpgsql(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ProcedureContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("ProcedureConnection"))
            .EnableSensitiveDataLogging(true));
    }

    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ILawyerRepository, LawyerRepository>();
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<IAuthManager, AuthManager>();
        builder.Services.AddTransient<IJwtTokenManager, JwtTokenManager>();

        builder.Services.AddTransient<ICaseContextService, CaseContextService>();

        builder.Services.AddTransient<ICasesContextRepository, CasesContextRepository>();
        builder.Services.AddTransient<ICasePartRepository, CasePartRepository>();

        builder.Services.AddTransient<ICaseRepository, CaseRepository>();

        builder.Services.AddTransient<IClientRepository, ClientRepository>();

    }

    private static void ConfigureHTTPLogging(WebApplicationBuilder builder)
    {
        _ = builder.Services.AddHttpLogging(logging =>
        {
            logging.LoggingFields = HttpLoggingFields.All;
            _ = logging.RequestHeaders.Add("sec-ch-ua");
            _ = logging.ResponseHeaders.Add("MyResponseHeader");
            logging.MediaTypeOptions.AddText("application/javascript");
            logging.RequestBodyLogLimit = 4096;
            logging.ResponseBodyLogLimit = 4096;
        });
    }

    private static void ConfigureDeserialization()
    {
    }
}
