using MediatR;
using WebComponentServer.Commands.ReverseProxy;
using WebComponentServer.Services.ReverseProxy;

namespace WebComponentServer.Handlers.ReverseProxy;

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
