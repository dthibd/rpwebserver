using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using MediatR;
using RPWebServerProto;
using RPWebSvrCli.Commands.Requests;
using RPWebSvrCli.Services;

namespace RPWebSvrCli.Commands.Handlers;

public class ShowToolVersionHandler : IRequestHandler<ShowToolVersionRequest>
{
    public ITextOutput TextOutput { get; }
    
    public ShowToolVersionHandler(ITextOutput textOutput)
    {
        TextOutput = textOutput;
    }
    
    public async Task Handle(ShowToolVersionRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var channel = GrpcChannel.ForAddress("https://127.0.0.1:8101");
            var client = new GetToolVersion.GetToolVersionClient(channel);
            
            var response = await client.GetToolVersionAsync(new Empty());
            
            TextOutput.WriteLine(response.Version);
            TextOutput.WriteLine("Tool version: 0.0.1");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        // return Task.CompletedTask;
    }
}

