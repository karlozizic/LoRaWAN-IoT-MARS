using System.Net.WebSockets;
using System.Text;
using ClientWS.Helpers;

namespace ClientWS.Core;

public static class ClientWs
{
    private static readonly Uri WebSocketUri = new Uri(Environment.GetEnvironmentVariable("URI")); //throws exception if environment var URI is null 
    
    public static async Task ConnectWebSocket()
    {
        
        using var webSocket = new ClientWebSocket();
        try
        {
            await webSocket.ConnectAsync(WebSocketUri, CancellationToken.None);

            await ReceiveMessages(webSocket);

            // Close the WebSocket connection
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed by the client",
                CancellationToken.None);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"WebSocket connection error: {ex.Message}");
        }
    }

    public static async Task ReceiveMessages(ClientWebSocket webSocket)
    {
        var buffer = new byte[1024];

        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None); /*new ArraySegment<byte>(buffer),*/
            
            if (result.MessageType == WebSocketMessageType.Text)
            {
                var message = ResultParser.ParseData(result, buffer); 
                Console.WriteLine($"Received message: {message}");
            }
        }
    }
    
    //TODO: add method that sends data to MARS
    /*public async Task SendData(ClientWebSocket webSocket)
    {    
    }*/
    
}