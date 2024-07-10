namespace AuthIdentity.Core.Options;

public class JwtOptions
{
    public string Key { get; set; }
    public TimeSpan Expire { get; set; }
    public string Audience { get; set; }
    public string Issuer { get; set; }
}