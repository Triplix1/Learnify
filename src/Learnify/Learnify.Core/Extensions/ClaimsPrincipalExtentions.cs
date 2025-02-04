using System.Security.Claims;

namespace Learnify.Core.Extensions;

public static class ClaimsPrincipalExtentions
{
    public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        var userIdString = claimsPrincipal.FindFirst("id")?.Value;
        int userId;

        if (userIdString is not null)
            userId = int.Parse(userIdString);
        else
            return -1;
        
        return userId;
    }
}