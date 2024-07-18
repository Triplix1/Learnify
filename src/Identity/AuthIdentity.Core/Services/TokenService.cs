using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthIdentity.Core.Domain.Entities;
using AuthIdentity.Core.Domain.RepositoryContracts;
using AuthIdentity.Core.Dto;
using AuthIdentity.Core.Options;
using AuthIdentity.Core.ServiceContracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthIdentity.Core.Services;

public class TokenService: ITokenService
{
    private readonly ILogger<TokenService> _logger;
    private readonly JwtOptions _jwtOptions;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public TokenService(IOptions<JwtOptions> jwtOptions, ILogger<TokenService> logger, IRefreshTokenRepository refreshTokenRepository)
    {
        _logger = logger;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtOptions = jwtOptions.Value;
    }
    
    public JwtSecurityToken GetDataFromToken(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);

        return token;
    }

    public string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("Name", user.Name),
            new Claim("Surname", user.Surname),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        JwtSecurityToken tokenGenerator = new(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            expires: DateTime.UtcNow.AddMilliseconds(_jwtOptions.Expire.TotalMilliseconds),
            signingCredentials: creds);

        var tokenHandler = new JwtSecurityTokenHandler();

        string token;
        try
        {
            token = tokenHandler.WriteToken(tokenGenerator);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "token generator threw exception");
            throw;
        }

        return token;
    }
    
    public string GenerateRefreshToken()
    {
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@#$%^&*()_+-=[]{}|;:,.<>?";

        // Generate a random byte array
        byte[] randomNumber = new byte[64];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(randomNumber);
        }

        // Convert the byte array to a character array
        char[] chars = new char[64];
        for (int i = 0; i < 64; i++)
        {
            int index = randomNumber[i] % validChars.Length;
            chars[i] = validChars[index];
        }

        return new string(chars);
    }
}