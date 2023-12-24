﻿using System.Net.WebSockets;
using System.Text;
using Microsoft.VisualBasic;

namespace ClientWS.Core;

public static class ClientWS
{
    //TODO: treat websocket uri as secret 
    private static readonly Uri WebSocketUri = new Uri("wss://smartinonet.oiv.hr/app?token=vgEAngAAABJzbWFydGlub25ldC5vaXYuaHJcgUBoeYcsll6iilTdfw9Q");
    
    public static async Task ConnectWebSocket()
    {
        using (var webSocket = new ClientWebSocket())
        {
            try
            {
                await webSocket.ConnectAsync(WebSocketUri, CancellationToken.None);

                await ReceiveMessage(webSocket);

                // Close the WebSocket connection
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed by the client",
                    CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WebSocket connection error: {ex.Message}");
            }
        }
    }

    public static async Task ReceiveMessage(ClientWebSocket webSocket)
    {
        var buffer = new byte[1024];

        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None); /*new ArraySegment<byte>(buffer),*/
            
            if (result.MessageType == WebSocketMessageType.Text)
            {
                var message = ParseData(result, buffer); 
                Console.WriteLine($"Received message: {message}");
            }
        }
    }
    
    //TODO: finish method for parsing data
    public static string ParseData(WebSocketReceiveResult result, byte[] buffer)
    {
        string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
        return message; 
    }
    
    //TODO: add method that sends data to MARS
    /*public async Task SendData(ClientWebSocket webSocket)
    {    
    }*/
    
}