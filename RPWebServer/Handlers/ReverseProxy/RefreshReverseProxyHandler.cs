using MediatR;
using RPWebServer.Commands.ReverseProxy;
using RPWebServer.Services.ReverseProxy;

namespace RPWebServer.Handlers.ReverseProxy;

public class RefreshReverseProxyHandler : IRequestHandler<RefreshReverseProxyRequest>
{
    private IReverseProxyChangesMonitor _changeMonitor;

    public RefreshReverseProxyHandler(IReverseProxyChangesMonitor changeMonitor)
    {
        _changeMonitor = changeMonitor;
    }
    
    public async Task Handle(RefreshReverseProxyRequest request, CancellationToken cancellationToken)
    {
        _changeMonitor.Update();
    }
}
