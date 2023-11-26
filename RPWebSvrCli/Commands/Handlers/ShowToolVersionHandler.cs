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
    
    public Task Handle(ShowToolVersionRequest request, CancellationToken cancellationToken)
    {
        try
        {
            TextOutput.WriteLine("Tool version: 0.0.1");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return Task.CompletedTask;
    }
}

