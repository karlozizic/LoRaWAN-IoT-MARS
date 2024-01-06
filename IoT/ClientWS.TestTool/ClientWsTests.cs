using ClientWS.Interfaces;

namespace ClientWS.TestTool;

public class ClientWsTests
{
    [Fact]
    public async Task TestReceiveMessage()
    {
        // Arrange
        var mockWebSocket = new Mock<IWebSocket>();
        var simulatedMessage = "{\"cmd\":\"rx\",\"seqno\":108294,\"EUI\":\"A81758FFFE04D146\",\"ts\":1704308311921,\"fcnt\":52001,\"port\":5,\"freq\":867100000,\"rssi\":-115,\"snr\":-0.2,\"toa\":287,\"dr\":\"SF8 BW125 4/5\",\"ack\":false,\"bat\":254,\"offline\":false,\"data\":\"0100ef02270400000502070dfa131409070a0b0e0f0c0e08080c0d0f11100c06080b110e0f0e0a080b0e0c0f0d0b0b080c0a0d0c0f0d0c08090b0b0e0a0a0b05070a0a090a090803030607080a0508\"}";
        //var simulatedMessage = "{\"cmd\":\"rx\",\"seqno\":108296,\"EUI\":\"A81758FFFE04D146\",\"ts\":1704308911887,\"fcnt\":52003,\"port\":5,\"freq\":868300000,\"rssi\":-114,\"snr\":-3.2,\"toa\":287,\"dr\":\"SF8 BW125 4/5\",\"ack\":false,\"bat\":254,\"offline\":false,\"data\":\"0100ef02270400000502070dfa13140a04090b0d110d0d07080c0d0f110f0b07060b0f0c0f0e0b0a090d0d100e0a0b060d0a0f0b100c0c07080b0c100a0a0b02050c0c0a09080a07030604070b0706\"}";
        //var simulatedMessage = "{"cmd":"rx","seqno":108297,"EUI":"A81758FFFE04D146","ts":1704309211867,"fcnt":52004,"port":5,"freq":867900000,"rssi":-109,"snr":-7,"toa":287,"dr":"SF8 BW125 4/5","ack":false,"bat":254,"offline":false,"data":"0100ef02270400000501070dfd13140a040a0c0d100d0d06060b0c0e100f0a06090b0f0c0f0d0a0b0a0e0e100d0c0a050b0b0e0a0f0a0c05080b0a0d0a090c03070b0b080c090b0604070609090606"}";
        //var simulatedMessage = "{"cmd":"rx","seqno":108300,"EUI":"A81758FFFE04D146","ts":1704309811830,"fcnt":52006,"port":5,"freq":867300000,"rssi":-113,"snr":1.5,"toa":287,"dr":"SF8 BW125 4/5","ack":false,"bat":254,"offline":false,"data":"0100ef02270400000501070dfa13140904090b0d100c0c06090b0c0d100d0908070b0f0d0f0c0c0b0b0c0d110e0a0b050c0b0f0c0f0c0b06080b0c0e0b090b04060c0c090b0a090703060608080506"}";
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