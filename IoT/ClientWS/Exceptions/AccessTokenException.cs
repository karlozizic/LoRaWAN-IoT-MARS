namespace ClientWS.Exceptions;

public class AccessTokenException : Exception
{
    public AccessTokenException(string message) : base(message)
    {
    }
}