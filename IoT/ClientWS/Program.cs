﻿using ClientWS.Core;

Console.WriteLine("Started console application..");
// add web socket uri as environment variable - URI=<web_socket_uri>
ClientWs.ConfigureClientWs();
await ClientWs.ConnectWebSocket(); 


