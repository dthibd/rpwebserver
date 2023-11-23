using AutoMapper;
using Moq;
using RPWebServer.Commands.ReverseProxy.Routes;
using RPWebServer.Handlers.ReverseProxy.Routes;
using RPWebServer.Models.RouteConfig;
using RPWebServer.Services.ReverseProxy;
using RPWebServer.Services.ReverseProxy.Config.Route;

namespace RPWebServerTest.Handlers.ReverseProxy.Routes;

public class UpdateRouteHandlerTest
{
    [Fact]
    public void Construction()
    {
        var routeConfigProviderMock = new Mock<IRoutesConfigProvider>();
        var mapperMock = new Mock<IMapper>();
        var handler = new UpdateRouteHandler(
            routeConfigProviderMock.Object,
            mapperMock.Object);

        Assert.Equal(routeConfigProviderMock.Object, handler.RoutesConfigProvider);
        Assert.Equal(mapperMock.Object, handler.Mapper);
    }

    [Fact]
    public async void Handler()
    {
        var request = new UpdateRouteRequest(
            new RouteConfigDto()
            {
                RouteId = "testId"
            });
        var routeConfigProviderMock = new Mock<IRoutesConfigProvider>();
        var mapperMock = new Mock<IMapper>();

        mapperMock
            .Setup(it => it.Map<MutableRouteConfig>(It.IsAny<RouteConfigDto>()))
            .Returns(new MutableRouteConfig("testId"));

        routeConfigProviderMock
            .Setup(it => it.Update(It.IsAny<MutableRouteConfig>()));
        
        var handler = new UpdateRouteHandler(
            routeConfigProviderMock.Object,
            mapperMock.Object);
        var response = await handler.Handle(request, new CancellationToken());

        Assert.NotNull(response);
        Assert.True(response.Succeeded);
        routeConfigProviderMock.Verify(it => it.Update(It.IsAny<MutableRouteConfig>()),
            Times.Once());
    }

    [Fact]
    public async void HandlerReturnsFailure()
    {
        var request = new UpdateRouteRequest(
            new RouteConfigDto()
            {
                RouteId = "testId"
            });
        var routeConfigProviderMock = new Mock<IRoutesConfigProvider>();
        var mapperMock = new Mock<IMapper>();
        var errorMsg = "invalid argument";

        mapperMock
            .Setup(it => it.Map<MutableRouteConfig>(It.IsAny<RouteConfigDto>()))
            .Returns(new MutableRouteConfig("testId"));

        routeConfigProviderMock
            .Setup(it => it.Update(It.IsAny<MutableRouteConfig>()))
            .Throws(new ArgumentException(errorMsg));
        
        var handler = new UpdateRouteHandler(
            routeConfigProviderMock.Object,
            mapperMock.Object);

        var response = await handler.Handle(request, new CancellationToken());

        Assert.NotNull(response);
        Assert.False(response.Succeeded);
        Assert.Equal(errorMsg, response.Error);
    }
}