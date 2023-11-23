using WebComponentServer.Services.ReverseProxy.Config.Cluster;
using Yarp.ReverseProxy.Configuration;

namespace WebComponentServer.Services.ReverseProxy;

public class CustomProxyConfigProvider : IProxyConfigProvider
{
    public ICustomMemoryConfig Config { get; set; }
    public ICustomMemoryConfigFactory ConfigFactory { get; }
    public IClustersConfigProvider ClustersConfigProvider { get; }
    public IRoutesConfigProvider RoutesConfigProvider { get; }
    public IReverseProxyChangesMonitor ChangeMonitor { get; }

    public CustomProxyConfigProvider(
        IClustersConfigProvider clustersConfigProvider, 
        IRoutesConfigProvider routesConfigProvider,
        IReverseProxyChangesMonitor changeMonitor,
        ICustomMemoryConfigFactory configFactory )
    {
        ClustersConfigProvider = clustersConfigProvider;
        RoutesConfigProvider = routesConfigProvider;
        ChangeMonitor = changeMonitor;
        ConfigFactory = configFactory;

        ChangeMonitor.UpdateObservable.Subscribe((value) =>
        {
            Update();
        });
        
        ClustersConfigProvider.CreateCluster("AngularWC").Set
            .LoadBalancingPolicy(LoadBalancingValue.RoundRobin)
            .AddDefaultDestination("http://localhost:8003");

        RoutesConfigProvider.Create("AngularWC").Set
            .ClusterId("AngularWC")
            .Match_CatchAllPath("test/url/angular")
            .Transforms_PathRemovePrefix("/test/url/angular");
        
        // Config = new CustomMemoryConfig(RoutesConfigProvider.ToRouteConfigList(), ClustersConfigProvider.ToClusterConfigList());
        Config = ConfigFactory.CreateInstance(RoutesConfigProvider.ToRouteConfigList(),
            ClustersConfigProvider.ToClusterConfigList());
    }

    public IProxyConfig GetConfig() => Config;

    /// <summary>
    /// By calling this method from the source we can dynamically adjust the proxy configuration.
    /// Since our provider is registered in DI mechanism it can be injected via constructors anywhere.
    /// </summary>
    public void Update(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
    {
        var oldConfig = Config;
        Config = new CustomMemoryConfig(routes, clusters);
        oldConfig.SignalChange();
    }

    public void Update()
    {
        var oldConfig = Config;
        // Config = new CustomMemoryConfig(RoutesConfigProvider.ToRouteConfigList(), ClustersConfigProvider.ToClusterConfigList());
        Config = ConfigFactory.CreateInstance(RoutesConfigProvider.ToRouteConfigList(), ClustersConfigProvider.ToClusterConfigList());
        oldConfig.SignalChange();
    }
}
