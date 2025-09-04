using Grpc.Core;
using GrpcServer.Protos;

namespace GrpcServer.Services;

public class GreeterService : Greeter.GreeterBase
{
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext ctx)
    {
        return Task.FromResult(new HelloReply
        {
            Message = $"Hello {request.Name}",
        });
    }
}
