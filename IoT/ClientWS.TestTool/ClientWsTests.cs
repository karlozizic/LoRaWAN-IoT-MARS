using ClientWS.Interfaces;

namespace ClientWS.TestTool;

public class ClientWsTests
{
    [Fact]
    public async Task TestReceiveMessage()
    {
        // Arrange
        var mockWebSocket = new Mock<IWebSocket>();
        //var simulatedMessage = "{\"cmd\":\"rx\",\"seqno\":105345,\"EUI\":\"A81758FFFE04D4AE\",\"ts\":1703418388974,\"fcnt\":28804,\"port\":5,\"freq\":868500000,\"rssi\":-115,\"snr\":-7.2,\"toa\":494,\"dr\":\"SF10 BW125 4/5\",\"ack\":false,\"bat\":254,\"offline\":false,\"data\":\"0100e6022503f20d3c070e1d0b000000410d000f001200\"}"; 
        //var simulatedMessage = "{\"cmd\":\"gw\",\"seqno\":105345,\"EUI\":\"A81758FFFE04D4AE\",\"ts\":1703418388974,\"fcnt\":28804,\"port\":5,\"freq\":868500000,\"toa\":494,\"dr\":\"SF10 BW125 4/5\",\"ack\":false,\"gws\":[{\"rssi\":-115,\"snr\":-7.2,\"ts\":1703418388974,\"time\":\"2023-12-24T11:46:28.974Z\",\"gweui\":\"024B0BFFFF032272\",\"ant\":0,\"lat\":45.7998336,\"lon\":16.0268288}],\"bat\":254,\"data\":\"0100e6022503f20d3c070e1d0b000000410d000f001200\"}";
        var simulatedMessage ="{\"cmd\":\"rx\",\"seqno\":105346,\"EUI\":\"A81758FFFE04D146\",\"ts\":1703418568694,\"fcnt\":49035,\"port\":5,\"freq\":868500000,\"rssi\":-117,\"snr\":-4.8,\"toa\":412,\"dr\":\"SF10 BW125 4/5\",\"ack\":false,\"bat\":254,\"offline\":false,\"data\":\"0100e602230400000503070df8\"}";
        //var simulatedMessage = "{\"cmd\":\"gw\",\"seqno\":105348,\"EUI\":\"A81758FFFE04D146\",\"ts\":1703419168658,\"fcnt\":49037,\"port\":5,\"freq\":867900000,\"toa\":412,\"dr\":\"SF10 BW125 4/5\",\"ack\":false,\"gws\":[{\"rssi\":-118,\"snr\":-1.8,\"ts\":1703419168658,\"time\":\"2023-12-24T11:59:28.658Z\",\"gweui\":\"024B0BFFFF032272\",\"ant\":0,\"lat\":45.7998336,\"lon\":16.0268288}],\"bat\":254,\"data\":\"0100e602230400000502070df8\"}";
        //var simulatedMessage ="{\"cmd\":\"rx\",\"seqno\":105343,\"EUI\":\"A81758FFFE04A758\",\"ts\":1703417968871,\"fcnt\":14172,\"port\":5,\"freq\":867700000,\"rssi\":-115,\"snr\":-1,\"toa\":453,\"dr\":\"SF10 BW125 4/5\",\"ack\":false,\"bat\":222,\"offline\":false,\"data\":\"0100e7022403000c3d040000070d440f00\"}"; 
        //var simulatedMessage = "{\"cmd\":\"gw\",\"seqno\":105359,\"EUI\":\"A81758FFFE04A758\",\"ts\":1703421568571,\"fcnt\":14175,\"port\":5,\"freq\":867700000,\"toa\":453,\"dr\":\"SF10 BW125 4/5\",\"ack\":false,\"gws\":[{\"rssi\":-114,\"snr\":-1.8,\"ts\":1703421568571,\"time\":\"2023-12-24T12:39:28.571Z\",\"gweui\":\"024B0BFFFF032272\",\"ant\":0,\"lat\":45.7998336,\"lon\":16.0268288}],\"bat\":222,\"data\":\"0100e7022403000c3d040000070d420f00\"}";
        
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
        var mockSendingService = new Mock<SendingService>();
        var clientService = new ClientWorker(mockSendingService.Object, mockWebSocket.Object, "wss://custom.uri");
        await clientService.ReceiveMessages();
    }
}