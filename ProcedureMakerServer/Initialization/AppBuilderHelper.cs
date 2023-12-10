
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Authentication.Interfaces;
using ProcedureMakerServer.Billing.Services;
using ProcedureMakerServer.Interfaces;
using ProcedureMakerServer.Repository;
using ProcedureMakerServer.Services;
using ProcedureMakerServer.Trusts;
using System.Reflection;
namespace ProcedureMakerServer.Initialization;



public static class AppBuilderHelper
{
    private static WebApplicationBuilder _builder;
    public static void Configure(WebApplicationBuilder builder)
    {
        ConfigureConventions(builder);
        _builder = builder;
        ConfigureNpgsql(builder);
        ConfigureServices(builder);
        ConfigureAutoMapper(builder);
        ConfigureHTTPLogging(builder);


    }
    private static void ConfigureConventions(WebApplicationBuilder builder) // required for my library for endpoints
    {
        _ = builder.Services.AddControllers(options =>
        {
            options.Conventions.Add(new LowercaseControllerModelConvention());
        });
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
        builder.Services.AddScoped<ILawyerRepository, LawyerRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IAuthManager, AuthManager>();
        builder.Services.AddScoped<IJwtTokenManager, JwtTokenManager>();
        builder.Services.AddScoped<ICaseContextService, CaseContextService>();
        builder.Services.AddScoped<ICasesContextRepository, CasesContextRepository>();
        builder.Services.AddScoped<ICasePartRepository, CasePartRepository>();
        builder.Services.AddScoped<ICaseRepository, CaseRepository>();
        builder.Services.AddScoped<IClientRepository, ClientRepository>();
        builder.Services.AddTransient<AccountStatementRepository>();
        builder.Services.AddTransient<TrustRepository>();
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
