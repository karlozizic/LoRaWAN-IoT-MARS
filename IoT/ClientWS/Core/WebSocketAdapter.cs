using System.Net.WebSockets;
using ClientWS.Interfaces;

namespace ClientWS.Core;

public class WebSocketAdapter : IWebSocket, IDisposable
{
    private readonly ClientWebSocket _webSocket = new();

    public async Task ConnectAsync(Uri? uri, CancellationToken cancellationToken)
    {
        await _webSocket.ConnectAsync(uri, cancellationToken);
    }

    public async Task<WebSocketReceiveResult> ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
    {
        return await _webSocket.ReceiveAsync(buffer, cancellationToken);
    }

    public async Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
    {
        await _webSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
    }

    public async Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
    {
        await _webSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);
    }

    public WebSocketState State => _webSocket.State;

    public void Dispose()
    {
        _webSocket.Dispose();
    }
}