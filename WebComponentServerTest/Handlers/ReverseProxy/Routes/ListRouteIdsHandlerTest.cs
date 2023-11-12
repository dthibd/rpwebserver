using Moq;
using WebComponentServer.Commands.ReverseProxy.Routes;
using WebComponentServer.Handlers.ReverseProxy.Routes;
using WebComponentServer.Services.ReverseProxy;

namespace WebComponentServerTest.Handlers.ReverseProxy.Routes;

public class ListRouteIdsHandlerTest
{
    public Mock<IRoutesConfigProvider> RoutesConfigProviderMock { get; } = new();

    [Fact]
    public void Construction()
    {
        var handler = new ListRouteIdsHandler(RoutesConfigProviderMock.Object);

        Assert.Equal(RoutesConfigProviderMock.Object, handler.RoutesConfigProvider);
    }

    [Fact]
    public async void Handle()
    {
        RoutesConfigProviderMock
            .Setup(it => it.ListRouteIds())
            .Returns(new List<string>() { "routeA", "routeB", "routeC" });

        var handler = new ListRouteIdsHandler(RoutesConfigProviderMock.Object);

        var result = await handler.Handle(
            new ListRouteIdsRequest(),
            new CancellationToken());

        Assert.NotNull(result);
        Assert.True(result.Succeeded);
        Assert.NotNull(result.Value);
        
        var idsList = result.Value;

        Assert.Equal(3, idsList.Count);
        Assert.Contains("routeA", idsList);
        Assert.Contains("routeB", idsList);
        Assert.Contains("routeC", idsList);
    }
}