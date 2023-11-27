using MediatR;
using Microsoft.Extensions.Logging;
using RPWebSvrCli.Commands.Requests;

namespace RPWebSvrCli.Commands.Handlers;

public class InitHandler : IRequestHandler<InitRequest>
{
    public ILogger<InitHandler> Logger { get; }
    
    public InitHandler(ILogger<InitHandler> logger)
    {
        Logger = logger;
    }
    
    public Task Handle(InitRequest request, CancellationToken cancellationToken)
    {
        Logger.LogDebug($"init request path : {request.Path}");

        return Task.CompletedTask;
    }
}
