using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Learnify.Core.Domain.Entities;
using Learnify.Core.Domain.RepositoryContracts;
using Learnify.Core.Dto.Auth;
using Learnify.Core.ManagerContracts;
using Learnify.Core.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Learnify.Core.Managers;

public class TokenManager: ITokenManager
{
    private readonly ILogger<TokenManager> _logger;
    private readonly JwtOptions _jwtOptions;

    public TokenManager(IOptions<JwtOptions> jwtOptions, ILogger<TokenManager> logger)
    {
        _logger = logger;
        _jwtOptions = jwtOptions.Value;
    }
    
    public JwtSecurityToken GetDataFromToken(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);

        return token;
    }

    public TokenResponse GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim("id", user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("name", user.Name),
            new Claim("surname", user.Surname),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddMilliseconds(_jwtOptions.Expire.TotalMilliseconds);
        
        JwtSecurityToken tokenGenerator = new(
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            claims,
            expires: expires,
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

        return new TokenResponse(token, expires);
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