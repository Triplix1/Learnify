using System.IdentityModel.Tokens.Jwt;
using Learnify.Core.Domain.Entities;
using Learnify.Core.Dto.Auth;

namespace Learnify.Core.ManagerContracts;

public interface ITokenManager
{
    TokenResponse GenerateJwtToken(User user);
    string GenerateRefreshToken();
    JwtSecurityToken GetDataFromToken(string jwt);
}