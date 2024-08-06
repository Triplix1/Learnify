using System.Security.Claims;

namespace Relaxinema.Core.Extentions;

public static class ClaimsPrincipalExtentions
{
    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        var userIdString = claimsPrincipal.FindFirst("id")?.Value;
        Guid userId;
        
        if (userIdString is not null)
            userId = Guid.Parse(userIdString);
        else
            throw new ApplicationException("User is not authorized");
        
        return userId;
    }
}