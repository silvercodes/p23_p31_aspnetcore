using GrpcServer;
using GrpcServer.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.ConfigureEndpointDefaults(listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});


var app = builder.Build();

app.MapGrpcService<GreeterService>();

app.MapGet("/", () => "Common HTTP Endpoint");



app.Run();
