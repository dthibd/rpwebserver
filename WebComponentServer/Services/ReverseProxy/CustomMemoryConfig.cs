using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace WebComponentServer.Services.ReverseProxy;

public class CustomMemoryConfig : IProxyConfig
{
    public readonly CancellationTokenSource CTS = new CancellationTokenSource();

    public CustomMemoryConfig(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
    {
        Routes = routes;
        Clusters = clusters;
        ChangeToken = new CancellationChangeToken(CTS.Token);
    }

    public IReadOnlyList<RouteConfig> Routes { get; }

    public IReadOnlyList<ClusterConfig> Clusters { get; }

    public IChangeToken ChangeToken { get; }
    public void SignalChange()
    {
        CTS.Cancel();
    }
    
}
