using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using ClientWS.Models;

namespace ClientWS.Helpers;

public static class ResultParser
{
    //TODO: finish method for parsing data
    public static string ParseData(WebSocketReceiveResult result, byte[] buffer)
    {
        string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
        var deserializedData = JsonSerializer.Deserialize<PayloadModel>(message);
        return message; 
    }
}