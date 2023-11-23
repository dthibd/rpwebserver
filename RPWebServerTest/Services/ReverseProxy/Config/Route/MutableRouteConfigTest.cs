using WebComponentServer.Services.ReverseProxy.Config.Route;

namespace WebComponentServerTest.Services.ReverseProxy.Config.Route;

public class MutableRouteConfigTest
{
    [Fact]
    public void Construction()
    {
        var mutableRouteConfig = new MutableRouteConfig("test");
        
        Assert.Equal("test", mutableRouteConfig.RouteId);
        Assert.NotNull(mutableRouteConfig.Set);
        Assert.NotNull(mutableRouteConfig.Match);
        Assert.NotNull(mutableRouteConfig.Transforms);
    }
    
    [Fact]
    public void ToRouteConfig()
    {
        var mutableRouteConfig = new MutableRouteConfig("test");
        mutableRouteConfig
            .Set
            .ClusterId("clusterId");

        var routeConfig = mutableRouteConfig.ToRouteConfig();

        Assert.NotNull(routeConfig);
        Assert.Equal("test", routeConfig.RouteId);
        Assert.Equal("clusterId", routeConfig.ClusterId);
    }
}