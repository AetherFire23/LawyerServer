using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ProcedureMakerServer.Authentication.AuthModels;
using System.Text;

namespace ProcedureMakerServer.Authentication;

public static class AuthenticationServiceExtensionscs
{
    public static void ConfigureJwt(this WebApplicationBuilder builder)
    {
        var jwtSection = builder.Configuration.GetSection("Jwt");
        var jwtConfig = new JwtConfig();
        jwtSection.Bind(jwtConfig);
        _ = builder.Services.Configure<JwtConfig>(jwtSection);

        // Configure JWT authentication
        _ = builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            // Set JWT bearer options
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidAudience = jwtConfig.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey))
            };
        });

        _ = builder.Services.AddAuthorization();
    }
}
