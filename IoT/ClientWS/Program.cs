
using ClientWS.Core;
using ClientWS.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// add web socket uri as environment variable - URI=<web_socket_uri>
Console.WriteLine("Started console application..");

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<SendingService>();
builder.Services.AddSingleton<IWebSocket, WebSocketAdapter>();
builder.Services.AddHostedService<ClientWorker>();
using IHost host = builder.Build(); 
host.Run();
    
