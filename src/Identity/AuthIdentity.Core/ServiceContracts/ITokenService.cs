using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AuthIdentity.Core.Domain.Entities;
using AuthIdentity.Core.Dto;

namespace AuthIdentity.Core.ServiceContracts;

public interface ITokenService
{
    string GenerateJwtToken(User user);
    string GenerateRefreshToken();
    JwtSecurityToken GetDataFromToken(string jwt);
}