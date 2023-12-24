using System.Net.WebSockets;
using System.Text;

namespace ClientWS.Helpers;

public static class ResultParser
{
    //TODO: finish method for parsing data
    public static string ParseData(WebSocketReceiveResult result, byte[] buffer)
    {
        string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
        return message; 
    }
}