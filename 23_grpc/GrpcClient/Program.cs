


using Grpc.Core;
using Grpc.Net.Client;
using GrpcServer.Protos;

using var channel = GrpcChannel.ForAddress("http://localhost:5194", new GrpcChannelOptions
{
    Credentials = ChannelCredentials.Insecure
});

Console.Write("Enter your name:");
var name = Console.ReadLine();

var client = new Greeter.GreeterClient(channel);

try
{
    var reply = await client.SayHelloAsync(new HelloRequest { Name = name });
    Console.WriteLine($"Server response: {reply.Message}");
}
catch (RpcException ex)
{
    Console.WriteLine($"gRPC Error: {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

System.Console.WriteLine("Press any key...");
Console.Read();
