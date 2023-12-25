using System.Net.WebSockets;
using ClientWS.Exceptions;
using ClientWS.Helpers;
using ClientWS.Interfaces;
using Microsoft.Extensions.Hosting;
using WebSocketException = System.Net.WebSockets.WebSocketException;

namespace ClientWS.Core;

public class ClientWorker : BackgroundService
{
    private SendingService _sendingService; 
    private static Uri WebSocketUri;
    private IWebSocket _webSocket;
    public ClientWorker(SendingService sendingService, IWebSocket webSocket, string? customUri = null)
    {
        _sendingService = sendingService;
        _webSocket = webSocket; 
        WebSocketUri = InitializeWebSocketUri(customUri);
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await ConnectWebSocket();
    }
    
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
        
        throw new WebSocketException("Invalid WebSocket URI.");
    }
    
    public async Task ConnectWebSocket()
    {
        try
        {
            await _webSocket.ConnectAsync(WebSocketUri, CancellationToken.None);
            
            await ReceiveMessages();

            // Close the WebSocket connection
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed by the client",
                CancellationToken.None);
            
            Console.WriteLine("Closed WebSocket connection");
        }
        catch (WebSocketException ex)
        {
            Console.WriteLine($"WebSocket connection error: {ex.Message}");
        }
    }

    public async Task ReceiveMessages()
    {
        var buffer = new byte[1024];

        while (_webSocket.State == WebSocketState.Open)
        {
            var result = await _webSocket.ReceiveAsync(buffer, CancellationToken.None);
            
            if (result.MessageType == WebSocketMessageType.Text)
            {
                try
                {
                    //parsing only Oxobutton data
                    var data = Parser.ParseData(result, buffer);
                }
                catch (PayloadDataException pde)
                {
                    Console.WriteLine($"{pde.Message}");
                }
                catch (InvalidDeviceException ide)
                {
                    // continue receiving messages 
                    Console.WriteLine($"{ide.Message}");
                }
            }
        }
    }
}
