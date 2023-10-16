using JWTAuth.Configs;
using JWTAuth.Interfaces;
using JWTAuth.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Authentication;

public abstract class JwtTokenManagerBase<TRoleDeclaration, TAuthenticationDb, TUser, TUserRole, TRole, TRoleType, TRegisterRequest> : IJwtTokenManager
       where TRoleDeclaration : RoleDeclarationBase<TRoleDeclaration, TUser, TUserRole, TRole, TRoleType>
       where TRoleType : struct, Enum
       where TUser : BaseUser<TRoleDeclaration, TUser, TUserRole, TRole, TRoleType>
       where TUserRole : BaseUserRole<TRoleDeclaration, TUserRole, TUser, TRole, TRoleType>
       where TRole : BaseRole<TRoleDeclaration, TRole, TUserRole, TUser, TRoleType>
       where TRegisterRequest : RegisterRequestBase<TRoleType>
{

    // make all this virtual so that I can override if necessary teh default configs

    private readonly JwtConfig _config;
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly TAuthenticationDb _authenticationDb;
    // IOptions retrievces jwetconfig from apsset
    public JwtTokenManagerBase(IOptions<JwtConfig> jwtConfig, TAuthenticationDb authDb)
    {
        _config = jwtConfig.Value;
        _tokenHandler = new JwtSecurityTokenHandler();
        _authenticationDb = authDb;
    }

    public async Task<string> GenerateToken(TUser user)
    {
        
        List<Claim> claims = await GetClaims(user);
        var signingCredentials = await GetSigningCredentials();
        DateTime expires = await GetExpirationTime();
        JwtSecurityToken securityToken = await GetSecurityToken(claims, expires, signingCredentials);
        var writenToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return writenToken;
    }

    public virtual async Task<SigningCredentials> GetSigningCredentials()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.SecretKey));
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        return creds;
    }

    public virtual async Task<DateTime> GetExpirationTime()
    {
        DateTime expires = DateTime.Now.AddDays(Convert.ToDouble(_config.ExpirationDays));
        return expires;
    }

    public virtual async Task<JwtSecurityToken> GetSecurityToken(List<Claim> claims, DateTime expires, SigningCredentials signingCredentials)
    {
        var securityToken = new JwtSecurityToken(
            _config.Issuer,
            _config.Audience,
            claims,
            expires: expires,
            signingCredentials: signingCredentials
        );

        return securityToken;
    }

    public virtual async Task<List<Claim>> GetClaims(TUser user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            //new Claim(ClaimTypes.Name.ToString, user.Name),
        };

        // create claims for each role
        var roleClaims = user.RoleTypes.Select(x => new Claim(ClaimTypes.Role, x.ToString())).ToList();
        claims.AddRange(roleClaims);

        return claims;
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
            return null;
        }
    }
}
