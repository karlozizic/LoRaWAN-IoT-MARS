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
    private static Uri? WebSocketUri;
    private IWebSocket _webSocket;
    //custom Uri in constructor added for Unit Test purposes - ClientWorker constructor is never called directly
    public ClientWorker(SendingService sendingService, IWebSocket webSocket, string? customUri = null)
    {
        _sendingService = sendingService;
        _webSocket = webSocket; 
        WebSocketUri = UriHelper.InitializeUri("WSS_URI", customUri);
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await ConnectWebSocket();
    }
    
    public async Task ConnectWebSocket()
    {
        try
        {
            await _webSocket.ConnectAsync(WebSocketUri, CancellationToken.None);
            
            await _sendingService.SetAccessToken(); 
            await ReceiveAndSendMessages();

            // Close the WebSocket connection
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed by the client",
                CancellationToken.None);
            _sendingService.Dispose();
            Console.WriteLine("Closed WebSocket connection");
        }
        catch (WebSocketException ex)
        {
            Console.WriteLine($"WebSocket connection error: {ex.Message}");
        }
    }

    public async Task ReceiveAndSendMessages()
    {
        var buffer = new byte[1024];

        while (_webSocket.State == WebSocketState.Open)
        {
            var result = await _webSocket.ReceiveAsync(buffer, CancellationToken.None);
            
            if (result.MessageType == WebSocketMessageType.Text)
            {
                try
                {
                    var data = Parser.ParseData(result, buffer);
                    await _sendingService.SendDataToMARS(data);
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
