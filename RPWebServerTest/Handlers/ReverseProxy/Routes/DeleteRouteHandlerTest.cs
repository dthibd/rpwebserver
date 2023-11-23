using Moq;
using WebComponentServer.Commands.ReverseProxy.Routes;
using WebComponentServer.Handlers.ReverseProxy.Routes;
using WebComponentServer.Services.ReverseProxy;

namespace WebComponentServerTest.Handlers.ReverseProxy.Routes;

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