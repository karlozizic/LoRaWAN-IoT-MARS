using ClientWS.Interfaces;

namespace ClientWS.TestTool;

public class ClientWsTests
{
    [Fact]
    public async Task TestReceiveMessage()
    {
        // Arrange
        var mockWebSocket = new Mock<IWebSocket>();
        var simulatedMessage = "{\"cmd\":\"gw\",\"seqno\":105345,\"EUI\":\"A81758FFFE04D4AE\",\"ts\":1703418388974,\"fcnt\":28804,\"port\":5,\"freq\":868500000,\"toa\":494,\"dr\":\"SF10 BW125 4/5\",\"ack\":false,\"gws\":[{\"rssi\":-115,\"snr\":-7.2,\"ts\":1703418388974,\"time\":\"2023-12-24T11:46:28.974Z\",\"gweui\":\"024B0BFFFF032272\",\"ant\":0,\"lat\":45.7998336,\"lon\":16.0268288}],\"bat\":254,\"data\":\"0100e6022503f20d3c070e1d0b000000410d000f001200\"}";
        var simulatedBytes = Encoding.UTF8.GetBytes(simulatedMessage);
        
        // Set the State property to Open to simulate an open WebSocket
        mockWebSocket.SetupGet(ws => ws.State).Returns(WebSocketState.Open);

        // Set up the mock behavior for ReceiveAsync
        mockWebSocket.Setup(ws => ws.ReceiveAsync(It.IsAny<ArraySegment<byte>>(), CancellationToken.None))
            .Callback<ArraySegment<byte>, CancellationToken>((buffer, token) =>
            {
                simulatedBytes.CopyTo(buffer.Array, 0);
            })
            .ReturnsAsync(new WebSocketReceiveResult(
                simulatedBytes.Length,
                WebSocketMessageType.Text,
                true));

        await ClientWs.ReceiveMessages(mockWebSocket.Object);
    }
}