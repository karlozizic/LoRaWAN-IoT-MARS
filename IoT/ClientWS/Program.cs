using System.Net.WebSockets;
using ClientWS.Core;

Console.WriteLine("Started console application..");
// add web socket uri as environment variable - URI=<web_socket_uri>
var clientWs = new WebSocketAdapter(new ClientWebSocket());
await ClientWs.ConnectWebSocket(clientWs);
clientWs.Dispose();
