using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProcedureMakerServer.Authentication.AuthModels;
using ProcedureMakerServer.Authentication.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProcedureMakerServer.Authentication;


public class JwtTokenManager : IJwtTokenManager
{
    private readonly JwtConfig _config;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public JwtTokenManager(IOptions<JwtConfig> config, JwtSecurityTokenHandler tokenHandler)
    {
        _config = config.Value;
        _tokenHandler = tokenHandler;
    }
    public async Task<string> GenerateToken(User user)
    {

        var roleClaims = user.Roles.Select(x => new Claim(ClaimTypes.Role, x.RoleType.ToString()));
        var claims = new List<Claim>(roleClaims)
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name)
        };

        // store security key
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.Now.AddDays(Convert.ToDouble(_config.ExpirationDays));

        var securityToken = new JwtSecurityToken(
            _config.Issuer,
            _config.Audience,
            claims,
            expires: expires,
            signingCredentials: creds
        );

        var writenToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return writenToken;
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        try
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                //ValidIssuer = _config.Issuer,
                //ValidAudience = _config.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.SecretKey))
            };

            // Validate and parse the token
            var principal = _tokenHandler.ValidateToken(token, validationParameters, out _);

            // Ensure the token has the required claims or perform additional validations

            return principal;
        }
        catch (Exception)
        {
            // Token validation failed
            return null;
        }
    }
}
