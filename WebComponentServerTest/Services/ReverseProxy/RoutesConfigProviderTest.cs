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
}