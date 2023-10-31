using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.LoadBalancing;

namespace WebComponentServer.Services.ReverseProxy;

public class CustomProxyConfigProvider : IProxyConfigProvider
{
    private CustomMemoryConfig _config;
    private IClustersConfigProvider _clustersConfigProvider;
    private IRoutesConfigProvider _routesConfigProvider;
    private IReverseProxyChangesMonitor _changeMonitor;

    public CustomProxyConfigProvider(
        IClustersConfigProvider clustersConfigProvider, 
        IRoutesConfigProvider routesConfigProvider,
        IReverseProxyChangesMonitor changeMonitor )
    {
        _clustersConfigProvider = clustersConfigProvider;
        _routesConfigProvider = routesConfigProvider;
        _changeMonitor = changeMonitor;

        _changeMonitor.UpdateObservable.Subscribe((value) =>
        {
            Update();
        });
        
        _clustersConfigProvider.CreateCluster("AngularWC").Set
            .LoadBalancingPolicy(LoadBalancingPolicies.RoundRobin)
            .AddDefaultDestination("http://localhost:8003");

        _routesConfigProvider.Create("AngularWC").Set
            .ClusterId("AngularWC")
            .Match_CatchAllPath("test/url/angular")
            .Transforms_PathRemovePrefix("/test/url/angular");
        
        _config = new CustomMemoryConfig(_routesConfigProvider.ToRouteConfigList(), _clustersConfigProvider.ToClusterConfigList());
    }

    public IProxyConfig GetConfig() => _config;

    /// <summary>
    /// By calling this method from the source we can dynamically adjust the proxy configuration.
    /// Since our provider is registered in DI mechanism it can be injected via constructors anywhere.
    /// </summary>
    public void Update(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
    {
        var oldConfig = _config;
        _config = new CustomMemoryConfig(routes, clusters);
        oldConfig.SignalChange();
    }

    public void Update()
    {
        var oldConfig = _config;
        _config = new CustomMemoryConfig(_routesConfigProvider.ToRouteConfigList(), _clustersConfigProvider.ToClusterConfigList());
        oldConfig.SignalChange();
    }
}
