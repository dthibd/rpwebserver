using System.Reactive.Subjects;
using Moq;
using WebComponentServer.Services.ReverseProxy;
using WebComponentServer.Services.ReverseProxy.Config.Cluster;
using WebComponentServer.Services.ReverseProxy.Config.Route;
using Yarp.ReverseProxy.Configuration;

namespace WebComponentServerTest.Services.ReverseProxy;

public class CustomProxyConfigProviderTest
{
    public Mock<IClustersConfigProvider> ClusterConfigProviderMock { get; }
    public Mock<IRoutesConfigProvider> RoutesConfigProviderMock { get; }
    public Mock<IReverseProxyChangesMonitor> ReverseProxyChangesMonitorMock { get; }
    public Mock<MutableClusterConfig> MutableClusterConfigMock { get; }
    public Mock<MutableRouteConfig> MutableRouteConfigMock { get; }

    public Subject<IReverseProxyChangesMonitor> ChangesMonitorSubject { get; } = new();
    
    public Mock<ICustomMemoryConfigFactory> ConfigFactoryMock { get; }
    public Mock<ICustomMemoryConfig> MemoryConfigMock { get; }
    
    public CustomProxyConfigProviderTest()
    {
        MutableClusterConfigMock = new Mock<MutableClusterConfig>("clusterId");
        MutableRouteConfigMock = new Mock<MutableRouteConfig>("routeId");

        var fluentMutableClusterConfigMock = new Mock<MutableClusterConfig.FluentMutableClusterConfig>(MutableClusterConfigMock.Object);
        fluentMutableClusterConfigMock
            .Setup(it => it.LoadBalancingPolicy(It.IsAny<LoadBalancingValue>()))
            .Returns(fluentMutableClusterConfigMock.Object);
        fluentMutableClusterConfigMock
            .Setup(it => it.AddDefaultDestination(It.IsAny<string>()))
            .Returns(fluentMutableClusterConfigMock.Object);

        var fluentMutableRouteConfigMock = new Mock<MutableRouteConfig.FluentMutableRouteConfig>(MutableRouteConfigMock.Object);
        fluentMutableRouteConfigMock
            .Setup(it => it.ClusterId(It.IsAny<string>()))
            .Returns(fluentMutableRouteConfigMock.Object);
        fluentMutableRouteConfigMock
            .Setup(it => it.Match_CatchAllPath(It.IsAny<string>()))
            .Returns(fluentMutableRouteConfigMock.Object);
        fluentMutableRouteConfigMock
            .Setup(it => it.Transforms_PathRemovePrefix(It.IsAny<string>()))
            .Returns(fluentMutableRouteConfigMock.Object);
        

        MutableClusterConfigMock
            .SetupGet(it => it.Set)
            .Returns(fluentMutableClusterConfigMock.Object);

        MutableRouteConfigMock
            .SetupGet(it => it.Set)
            .Returns(fluentMutableRouteConfigMock.Object);

        ClusterConfigProviderMock = new Mock<IClustersConfigProvider>();
        ClusterConfigProviderMock
            .Setup(it => it.CreateCluster(It.IsAny<string>()))
            .Returns(MutableClusterConfigMock.Object);

        RoutesConfigProviderMock = new Mock<IRoutesConfigProvider>();
        RoutesConfigProviderMock
            .Setup(it => it.Create(It.IsAny<string>()))
            .Returns(MutableRouteConfigMock.Object);

        ReverseProxyChangesMonitorMock = new Mock<IReverseProxyChangesMonitor>();
        ReverseProxyChangesMonitorMock
            .SetupGet(it => it.UpdateObservable)
            .Returns(ChangesMonitorSubject);

        MemoryConfigMock = new Mock<ICustomMemoryConfig>();
        ConfigFactoryMock = new Mock<ICustomMemoryConfigFactory>();
        ConfigFactoryMock
            .Setup(it =>
                it.CreateInstance(It.IsAny<IReadOnlyList<RouteConfig>>(), It.IsAny<IReadOnlyList<ClusterConfig>>()))
            .Returns(MemoryConfigMock.Object);
    }

    [Fact]
    public void Construction()
    {
        var provider = new CustomProxyConfigProvider(
            ClusterConfigProviderMock.Object,
            RoutesConfigProviderMock.Object,
            ReverseProxyChangesMonitorMock.Object,
            ConfigFactoryMock.Object);

        Assert.NotNull(provider.Config);
        Assert.NotNull(provider.GetConfig());
    }

    [Fact]
    public void ChangesTriggeredByMonitor()
    {
        MemoryConfigMock
            .Setup(it => it.SignalChange());
        
        var provider = new CustomProxyConfigProvider(
            ClusterConfigProviderMock.Object,
            RoutesConfigProviderMock.Object,
            ReverseProxyChangesMonitorMock.Object,
            ConfigFactoryMock.Object);

        ChangesMonitorSubject.OnNext(ReverseProxyChangesMonitorMock.Object);

        MemoryConfigMock.Verify(it => it.SignalChange(), Times.Once);
    }

    [Fact]
    public void UpdateTriggersChange()
    {
        MemoryConfigMock
            .Setup(it => it.SignalChange());
        
        var provider = new CustomProxyConfigProvider(
            ClusterConfigProviderMock.Object,
            RoutesConfigProviderMock.Object,
            ReverseProxyChangesMonitorMock.Object,
            ConfigFactoryMock.Object);

        provider.Update(new List<RouteConfig>(), new List<ClusterConfig>());

        MemoryConfigMock.Verify(it => it.SignalChange(), Times.Once);
        
    }
}
