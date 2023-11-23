using WebComponentServer.Services.ReverseProxy;
using Yarp.ReverseProxy.Configuration;

namespace WebComponentServerTest.Services.ReverseProxy;

public class CustomMemoryConfigTest
{
    [Fact]
    public void Construction()
    {
        var routeConfig = new RouteConfig()
        {
            RouteId = "routeId"
        };
        var clusterConfig = new ClusterConfig()
        {
            ClusterId = "clusterId"
        };
        
        var config = new CustomMemoryConfig(
            new List<RouteConfig>()
            {
                routeConfig
            },
            new List<ClusterConfig>()
            {
                clusterConfig
            });

        Assert.Single(config.Routes);
        Assert.Single(config.Clusters);
        Assert.NotNull(config.ChangeToken);
    }

    [Fact]
    public void SignalChange()
    {
        var routeConfig = new RouteConfig()
        {
            RouteId = "routeId"
        };
        var clusterConfig = new ClusterConfig()
        {
            ClusterId = "clusterId"
        };
        
        var config = new CustomMemoryConfig(
            new List<RouteConfig>()
            {
                routeConfig
            },
            new List<ClusterConfig>()
            {
                clusterConfig
            });

        config.SignalChange();
        
        Assert.True(config.CTS.IsCancellationRequested);
    }
}
