namespace AuthIdentity.Core.Dto;

/// <summary>
/// Record that represents a token response 
/// </summary>
/// <param name="Token">Access token</param>
/// <param name="Expires">Expiration time</param>
public sealed record TokenResponse(string Token, DateTime Expires)
{
}