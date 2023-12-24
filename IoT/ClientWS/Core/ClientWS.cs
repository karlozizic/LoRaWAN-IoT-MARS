using System.Net.WebSockets;
using ClientWS.Helpers;
using ClientWS.Interfaces;

namespace ClientWS.Core;

public static class ClientWs
{
    private static Uri WebSocketUri;
    
    private static Uri InitializeWebSocketUri(string? customUri = null)
    {
        
        if (Uri.TryCreate(customUri, UriKind.Absolute, out var uri))
        {
            return new Uri(customUri);
        }
        
        string uriString = Environment.GetEnvironmentVariable("URI");
        
        if (!string.IsNullOrEmpty(uriString))
        {
            return new Uri(uriString);
        }
        
        throw new Exception("Invalid WebSocket URI.");
    }
    
    public static async Task ConnectWebSocket(IWebSocket webSocket, string? webSocketUri = null)
    {
        try
        {
            WebSocketUri = InitializeWebSocketUri(webSocketUri);
            await webSocket.ConnectAsync(WebSocketUri, CancellationToken.None);

            await ReceiveMessages(webSocket);

            // Close the WebSocket connection
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed by the client",
                CancellationToken.None);
            Console.WriteLine("Closed WebSocket connection");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"WebSocket connection error: {ex.Message}");
        }
    }

    public static async Task ReceiveMessages(IWebSocket webSocket)
    {
        var buffer = new byte[1024];

        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None); /*new ArraySegment<byte>(buffer),*/
            
            if (result.MessageType == WebSocketMessageType.Text)
            {
                var message = ResultParser.ParseData(result, buffer); 
                Console.WriteLine($"Received message: {message} at time {DateTime.Now}");
            }
        }
    }
    
    //TODO: add method that sends data to MARS
    /*private async Task SendData(ClientWebSocket webSocket)
    {    
    }*/
    
}
