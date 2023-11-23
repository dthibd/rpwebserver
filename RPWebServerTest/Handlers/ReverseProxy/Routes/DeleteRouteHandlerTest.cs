using Moq;
using RPWebServer.Commands.ReverseProxy.Routes;
using RPWebServer.Handlers.ReverseProxy.Routes;
using RPWebServer.Services.ReverseProxy;

namespace RPWebServerTest.Handlers.ReverseProxy.Routes;

public class DeleteRouteHandlerTest
{
    public Mock<IRoutesConfigProvider> RoutesConfigProviderMock { get; } = new();

    [Fact]
    public void Construct()
    {
        var handler = new DeleteRouteHandler(RoutesConfigProviderMock.Object);
        
        Assert.Equal(RoutesConfigProviderMock.Object, handler.RoutesConfigProvider);
    }

    [Fact]
    public async void RemoveExistingRoute()
    {
        RoutesConfigProviderMock
            .Setup(it => it.Remove(It.IsAny<string>()));
        
        var handler = new DeleteRouteHandler(RoutesConfigProviderMock.Object);

        var result = await handler.Handle(
            new DeleteRouteRequest("someId"),
            new CancellationToken());

        Assert.NotNull(result);
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async void RemoveInvalidId()
    {
        RoutesConfigProviderMock
            .Setup(it => it.Remove(It.IsAny<string>()))
            .Throws<ArgumentException>();

        var handler = new DeleteRouteHandler(RoutesConfigProviderMock.Object);

        var result = await handler.Handle(
            new DeleteRouteRequest("someId"),
            new CancellationToken());

        Assert.NotNull(result);
        Assert.False(result.Succeeded);
    }
}