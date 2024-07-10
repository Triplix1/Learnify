using AuthIdentity.Core.Enums;

namespace AuthIdentity.Core.Dto;

public class GoogleAuthRequest
{
    public string Code { get; set; }
    public string CodeVerifier { get; set; }
    public string RedirectUrl { get; set; }
    public RoleRequest Role { get; set; }
}