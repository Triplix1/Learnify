using System.Security.Claims;

namespace Learnify.Core.Extensions;

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