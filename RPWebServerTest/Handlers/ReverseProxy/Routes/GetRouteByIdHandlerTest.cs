using AutoMapper;
using Moq;
using WebComponentServer.Commands.ReverseProxy.Routes;
using WebComponentServer.Handlers.ReverseProxy.Routes;
using WebComponentServer.Models.RouteConfig;
using WebComponentServer.Services.ReverseProxy;
using WebComponentServer.Services.ReverseProxy.Config.Route;

namespace WebComponentServerTest.Handlers.ReverseProxy.Routes;

public class GetRouteByIdHandlerTest
{
    public Mock<IRoutesConfigProvider> RoutesConfigProviderMock { get; } = new();

    public Mock<IMapper> MapperMock { get; } = new();
    
    public string[] Routes { get; }
    public MutableRouteConfig[] MutableRouteConfigs { get; }
    public RouteConfigDto[] RouteConfigsDto { get; }

    public GetRouteByIdHandlerTest()
    {
        Routes = new[] { "routeA", "routeB" };
        MutableRouteConfigs = new[]
        {
            new MutableRouteConfig(Routes[0]),
            new MutableRouteConfig(Routes[1])
        };
        RouteConfigsDto = new[]
        {
            new RouteConfigDto() { RouteId = Routes[0] },
            new RouteConfigDto() { RouteId = Routes[1] }
        };

        RoutesConfigProviderMock
            .Setup(it => it.GetRouteById(Routes[0]))
            .Returns(MutableRouteConfigs[0]);
        
        RoutesConfigProviderMock
            .Setup(it => it.GetRouteById(Routes[1]))
            .Returns(MutableRouteConfigs[1]);

        RoutesConfigProviderMock
            .Setup<MutableRouteConfig?>(it => it.GetRouteById(It.IsNotIn(Routes)));

        MapperMock
            .Setup(it => it.Map<RouteConfigDto>(MutableRouteConfigs[0]))
            .Returns(RouteConfigsDto[0]);
        
        MapperMock
            .Setup(it => it.Map<RouteConfigDto>(MutableRouteConfigs[1]))
            .Returns(RouteConfigsDto[1]);
    }
    
    [Fact]
    public void Construction()
    {
        var handler = new GetRouteByIdHandler(
            RoutesConfigProviderMock.Object,
            MapperMock.Object);
        
        Assert.Equal(RoutesConfigProviderMock.Object, handler.RoutesConfigProvider);
        Assert.Equal(MapperMock.Object, handler.Mapper);
    }

    [Fact]
    public async void HandleWithValidRoute()
    {
        var handler = new GetRouteByIdHandler(
            RoutesConfigProviderMock.Object,
            MapperMock.Object);

        var result = await handler.Handle(
            new GetRouteByIdRequest("routeB"), 
            new CancellationToken());

        Assert.NotNull(result);
        Assert.True(result.Succeeded);
        Assert.Equal(RouteConfigsDto[1], result.Value);
    }

    [Fact]
    public async void HandleWithInvalidRoute()
    {
        var handler = new GetRouteByIdHandler(
            RoutesConfigProviderMock.Object,
            MapperMock.Object);

        var result = await handler.Handle(
            new GetRouteByIdRequest("routeC"), 
            new CancellationToken());

        Assert.NotNull(result);
        Assert.True(result.Succeeded);
        Assert.Null(result.Value);
    }
}