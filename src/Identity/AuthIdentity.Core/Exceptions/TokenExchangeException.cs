namespace AuthIdentity.Core.Exceptions;

public class TokenExchangeException : Exception
{
    public TokenExchangeException() : base("Exception occured while exchanging token")
    {
        
    }
}