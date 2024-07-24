namespace AuthIdentity.Core.Dto;

public class AuthResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expires { get; set; }
}