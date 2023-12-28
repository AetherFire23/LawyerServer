
using HtmlRenderFun;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using ProcedureMakerServer.Authentication;
using ProcedureMakerServer.Authentication.Interfaces;
using ProcedureMakerServer.Billing.Services;
using ProcedureMakerServer.EmailMaker;
using ProcedureMakerServer.Repository;
using ProcedureMakerServer.Services;
using ProcedureMakerServer.TemplateManagement;
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
		_ = builder.Services.AddAutoMapper(x =>
		{
			x.AddMaps(Assembly.GetExecutingAssembly());
		});
	}

	public static void ConfigureNpgsql(WebApplicationBuilder builder)
	{
		//builder.Services.AddDbContext<ProcedureContext>(options =>
		//    options.UseNpgsql(builder.Configuration.GetConnectionString("ProcedureConnection"))
		//    .EnableSensitiveDataLogging(true));

		_ = builder.Services.AddSqlServer<ProcedureContext>(builder.Configuration.GetConnectionString("ProcedureConnection"));
	}

	public static void ConfigureServices(WebApplicationBuilder builder)
	{
		_ = builder.Services.AddScoped<LawyerRepository>();
		_ = builder.Services.AddScoped<UserRepository>();
		_ = builder.Services.AddScoped<IAuthManager, AuthManager>();
		_ = builder.Services.AddScoped<IJwtTokenManager, JwtTokenManager>();
		_ = builder.Services.AddScoped<CaseContextService>();
		_ = builder.Services.AddScoped<CaseContextRepository>();
		_ = builder.Services.AddScoped<CasePartRepository>();
		_ = builder.Services.AddScoped<CaseRepository>();
		_ = builder.Services.AddScoped<ClientRepository>();
		_ = builder.Services.AddTransient<AccountStatementRepository>();
		_ = builder.Services.AddTransient<TrustRepository>();
		_ = builder.Services.AddTransient<NotificationEmailSender>();
		_ = builder.Services.AddTransient<DocumentMakerService>();
		_ = builder.Services.AddTransient<NotificationService>();
		_ = builder.Services.AddTransient<InvoiceRepository>();
		_ = builder.Services.AddTransient<ProcedureHtmlRenderer>();

		//_ = builder.Services.AddTransient<ProcedureHtmlRenderer>();


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
}
