using Moq;
using WebComponentServer.Services.ReverseProxy;
using WebComponentServer.Services.ReverseProxy.Config.Route;
using Yarp.ReverseProxy.Configuration;

namespace WebComponentServerTest.Services.ReverseProxy;

public class RoutesConfigProviderTest
{
    private MutableRouteConfig TestRouteConfig { get; }

    private string RouteId { get; } = "TestId";
    private string ClusterId { get; } = "ClusterId";
    private string CatchAllPath { get; } = "/match/path";
    private string PathRemovePrefix { get; } = "/path/remove/prefix";

    public RoutesConfigProviderTest()
    {
        TestRouteConfig = new MutableRouteConfig(RouteId);
        TestRouteConfig.Set
            .ClusterId(ClusterId)
            .Match_CatchAllPath(CatchAllPath)
            .Transforms_PathRemovePrefix(PathRemovePrefix);
    }

    [Fact]
    public void ToRouteConfigWithNoItems()
    {
        var provider = new RoutesConfigProvider();
        var routes = provider.ToRouteConfigList();

        Assert.NotNull(routes);
        Assert.Empty(routes);
    }

    [Fact]
    public void ToRouteConfigWithOneItem()
    {
        var provider = new RoutesConfigProvider();
        provider.Routes.Add(RouteId, TestRouteConfig);

        var routes = provider.ToRouteConfigList();

        Assert.NotNull(routes);
        Assert.Single(routes);
    }

    [Fact]
    public void ToRouteConfigCallToToRouteConfigIsDoneInSelect()
    {
        var routeConfigMock = new Mock<MutableRouteConfig>(RouteId);

        routeConfigMock
            .Setup(it => it.ToRouteConfig())
            .Returns(new RouteConfig
            {
                RouteId = RouteId
            });
        
        var provider = new RoutesConfigProvider();
        provider.Routes.Add(RouteId, routeConfigMock.Object);

        var routes = provider.ToRouteConfigList();

        Assert.NotNull(routes);
        routeConfigMock.Verify(it => it.ToRouteConfig(), Times.Once());
    }

    [Fact]
    public void CreateRouteConfigWithValidId()
    {
        var provider = new RoutesConfigProvider();
        var routeConfig = provider.Create(RouteId);

        Assert.NotNull(routeConfig);
        Assert.Single(provider.Routes);
    }

    [Fact]
    public void CreateRouteConfigWithExistingId()
    {
        var provider = new RoutesConfigProvider();
        provider.Routes.Add(RouteId, TestRouteConfig);

        Assert.Throws<ArgumentException>(() => provider.Create(RouteId));
    }

    [Fact]
    public void AddMutableRouteConfig()
    {
        var provider = new RoutesConfigProvider();
        provider.Add(TestRouteConfig);

        Assert.Single(provider.Routes);

        var entry = provider.Routes.First();

        Assert.Equal(RouteId, entry.Key);
    }

    [Fact]
    public void AddMutableRouteConfigWithIdAlreadyPresent()
    {
        var provider = new RoutesConfigProvider();
        provider.Routes.Add(RouteId, TestRouteConfig);

        Assert.Throws<ArgumentException>(() => provider.Add(TestRouteConfig));
    }

    [Fact]
    public void UpdateWithNonExistingConfigThrows()
    {
        var provider = new RoutesConfigProvider();

        Assert.Throws<ArgumentException>(() => provider.Update(TestRouteConfig));
    }

    [Fact]
    public void UpdateWithExistingConfigSetNewOnes()
    {
        var provider = new RoutesConfigProvider();
        provider.Routes.Add(RouteId, new MutableRouteConfig(RouteId));

        var firstEntry = provider.Routes.First();

        Assert.NotNull(firstEntry.Value);
        Assert.Null(firstEntry.Value.ClusterId);
        
        provider.Update(TestRouteConfig);
        
        firstEntry = provider.Routes.First();

        Assert.NotNull(firstEntry.Value);
        Assert.Equal(ClusterId, firstEntry.Value.ClusterId);
    }

    [Fact]
    public void RemoveNonExistingIdThrows()
    {
        var provider = new RoutesConfigProvider();

        Assert.Throws<ArgumentException>(() => provider.Remove(RouteId));
    }

    [Fact]
    public void RemoveExistingId()
    {
        var provider = new RoutesConfigProvider();
        provider.Routes.Add(RouteId, TestRouteConfig);

        Assert.Single(provider.Routes);

        provider.Remove(RouteId);

        Assert.Empty(provider.Routes);
    }

    [Fact]
    public void ListRoutesIds()
    {
        var provider = new RoutesConfigProvider();
        
        provider.Add(new MutableRouteConfig("routeA"));
        provider.Add(new MutableRouteConfig("routeB"));
        
        var ids = provider.ListRouteIds();

        Assert.Equal(2, provider.Routes.Count);
        Assert.Contains("routeA", ids);
        Assert.Contains("routeB", ids);
    }

    [Fact]
    public void GetRouteByIdWithExistingId()
    {
        var provider = new RoutesConfigProvider();

        provider.Add(new MutableRouteConfig("routeA"));
        provider.Add(new MutableRouteConfig("routeB"));

        var route = provider.GetRouteById("routeB");

        Assert.NotNull(route);
        Assert.Equal("routeB", route.RouteId);
    }

    [Fact]
    public void GetRouteByIdWithInvalidId()
    {
        var provider = new RoutesConfigProvider();

        provider.Add(new MutableRouteConfig("routeA"));
        provider.Add(new MutableRouteConfig("routeB"));

        var route = provider.GetRouteById("routeC");

        Assert.Null(route);
    }

    [Fact]
    public void ListRoutes()
    {
        var provider = new RoutesConfigProvider();

        provider.Add(new MutableRouteConfig("routeA"));
        provider.Add(new MutableRouteConfig("routeB"));

        var routes = provider.ListRoutes();

        Assert.Equal(2, routes.Count);
        Assert.Contains(routes, item => item.RouteId == "routeB");
        Assert.Contains(routes, item => item.RouteId == "routeA");
    }
}