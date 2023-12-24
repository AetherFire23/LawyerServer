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

    public JwtTokenManager(IOptions<JwtConfig> config)
    {
        _config = config.Value;
        _tokenHandler = new JwtSecurityTokenHandler();

    }



    public async Task<string> GenerateToken(User user)
    {
        await Task.Delay(0);

        List<RoleTypes> roles = user.UserRoles.Select(x => x.Role.RoleType).ToList();

        IEnumerable<Claim> roleClaims = user.Roles.Select(r => new Claim(ClaimTypes.Role, r.ToString()));
        List<Claim> claims = new List<Claim>(roleClaims)
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
        };

        // store security key
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.SecretKey));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        DateTime expires = DateTime.Now.AddDays(Convert.ToDouble(_config.ExpirationDays));

        JwtSecurityToken securityToken = new JwtSecurityToken(
            _config.Issuer,
            _config.Audience,
            claims,
            expires: expires,
            signingCredentials: creds
        );

        string writenToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return writenToken;
    }

    public async Task<ClaimsPrincipal> ValidateToken(string token)
    {
        try
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _config.Issuer,
                ValidAudience = _config.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.SecretKey))
            };

            // Validate and parse the token
            ClaimsPrincipal principal = _tokenHandler.ValidateToken(token, validationParameters, out _);

            // Ensure the token has the required claims or perform additional validations

            return principal;
        }
        catch (Exception)
        {
            // Token validation failed
            return null!;
        }
    }
}
