using MediatR;
using Microsoft.Extensions.Logging;
using RPWebSvrCli.Commands.Requests;

namespace RPWebSvrCli.Services;

public class Worker : IWorker
{
    public ILogger<IWorker> Logger { get; }
    
    public ITextOutput TextOutput { get; }
    
    public IMediator Mediator { get; }
    
    public Worker(
        ILogger<IWorker> logger,
        ITextOutput textOutput,
        IMediator mediator
        )
    {
        Logger = logger;
        TextOutput = textOutput;
        Mediator = mediator;
    }

    public void HandleCommandLineOptions(CommandLineOptions options)
    {
        if (options.ToolVersion)
        {
            OnToolVersion();   
        }

        if (options.Init)
        {
            OnInit();
        }
    }

    public void OnToolVersion()
    {
        Mediator.Send(new ShowToolVersionRequest());
    }

    public void OnInit()
    {
        Mediator.Send(new InitRequest(Directory.GetCurrentDirectory()));
    }
}
