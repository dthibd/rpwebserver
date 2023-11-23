using WebComponentServer.Services.ReverseProxy.Config.Route;

namespace WebComponentServerTest.Services.ReverseProxy.Config.Route;

public class MutableRouteMatchTest
{
    [Fact]
    public void SetCatchAllWithBasicPath()
    {
        var routeMatch = new MutableRouteMatch();
        routeMatch.SetCatchAllPath("/path/test");
        
        Assert.Equal("/path/test/{**catch-all}", routeMatch.Path);
    }

    
    [Fact]
    public void SetCatchAllWithPathContainingCatchAll()
    {
        var routeMatch = new MutableRouteMatch();
        routeMatch.SetCatchAllPath("/path/test/{**catch-all}");
        
        Assert.Equal("/path/test/{**catch-all}", routeMatch.Path);
    }

}
