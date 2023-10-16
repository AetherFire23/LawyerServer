using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuth.Configs;
public class JwtConfig
{
    public string SecretKey { get; set; } = string.Empty; // un hash
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty; // verification soources de tokens
    public int ExpirationDays { get; set; }
}
