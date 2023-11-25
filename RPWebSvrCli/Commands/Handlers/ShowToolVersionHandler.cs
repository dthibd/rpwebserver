using Grpc.Net.Client;
using MediatR;
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
        var channel = GrpcChannel.ForAddress("http://localhost:8100");
        var client = new GetToolVersion.GetToolVersionClient(channel);

        var response = await client.GetToolVersionAsync(
            new GetToolVersionRequest());

        TextOutput.WriteLine(response.Version);
        TextOutput.WriteLine("Tool version: 0.0.1");

        // return Task.CompletedTask;
    }
}
