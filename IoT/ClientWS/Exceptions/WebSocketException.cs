namespace ClientWS.Exceptions;

public class WebSocketException : Exception
{
    public WebSocketException(string message) : base(message)
    {
    }
}