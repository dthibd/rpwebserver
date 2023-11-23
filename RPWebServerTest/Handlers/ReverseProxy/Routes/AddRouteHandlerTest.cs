using AutoMapper;
using Moq;
using RPWebServer.Commands.ReverseProxy.Routes;
using RPWebServer.Handlers.ReverseProxy.Routes;
using RPWebServer.Models.RouteConfig;
using RPWebServer.Services.ReverseProxy;
using RPWebServer.Services.ReverseProxy.Config.Route;

namespace RPWebServerTest.Handlers.ReverseProxy.Routes;

public class AddRouteHandlerTest
{
    public Mock<IRoutesConfigProvider> RoutesConfigProviderMock { get; } = new();
    public Mock<IMapper> MapperMock { get; } = new();

    public RouteConfigDto RouteCfgDto { get; } = new() { RouteId = "someId" };

    public MutableRouteConfig MutableRouteCfg { get; } = new("someId");

    public AddRouteHandlerTest()
    {
        MapperMock
            .Setup(it => it.Map<MutableRouteConfig>(RouteCfgDto))
            .Returns(MutableRouteCfg);
    }
    
    [Fact]
    public void Construct()
    {
        var handler = new AddRouteHandler(
            RoutesConfigProviderMock.Object,
            MapperMock.Object);

        Assert.Equal(RoutesConfigProviderMock.Object, handler.RoutesConfigProvider);
        Assert.Equal(MapperMock.Object, handler.Mapper);
    }

    [Fact]
    public async void HandleWithValidRoute()
    {
        RoutesConfigProviderMock
            .Setup(it => it.Add(MutableRouteCfg));

        var handler = new AddRouteHandler(
            RoutesConfigProviderMock.Object,
            MapperMock.Object);

        var result = await handler.Handle(
            new AddRouteRequest(RouteCfgDto),
            new CancellationToken());

        Assert.NotNull(result);
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async void HandleWithInvalidRoute()
    {
        RoutesConfigProviderMock
            .Setup(it => it.Add(MutableRouteCfg))
            .Throws<ArgumentException>();

        var handler = new AddRouteHandler(
            RoutesConfigProviderMock.Object,
            MapperMock.Object);

        var result = await handler.Handle(
            new AddRouteRequest(RouteCfgDto),
            new CancellationToken());

        Assert.NotNull(result);
        Assert.False(result.Succeeded);
    }
}