namespace ProcedureMakerServer.Authentication.AuthModels;

public class JwtConfig
{
    public string SecretKey { get; set; } = string.Empty; // un hash
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty; // verification soources de tokens
    public int ExpirationDays { get; set; }
}
