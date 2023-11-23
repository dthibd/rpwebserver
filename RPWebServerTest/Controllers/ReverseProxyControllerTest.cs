using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RPWebServer.Commands.Responses;
using RPWebServer.Commands.ReverseProxy.Routes;
using RPWebServer.Controllers;
using RPWebServer.Models.RouteConfig;

namespace RPWebServerTest.Controllers;

public class ReverseProxyControllerTest
{
    public Mock<IMediator> MediatorMock { get; set; } = new();

    [Fact]
    public void Construction()
    {
        var controller = new ReverseProxyController(MediatorMock.Object);
        
        Assert.Equal(MediatorMock.Object, controller.Mediator);
    }
    
    [Fact]
    public async void ListRouteIds()
    {
        var controller = new ReverseProxyController(MediatorMock.Object);
        var response = new ListRouteIdsRequestResponse(new List<string>()
        {
            "route1",
            "route2"
        });

        MediatorMock
            .Setup(it => it.Send(It.IsAny<ListRouteIdsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        
        var result = await controller.ListRouteIds();

        Assert.NotNull(result);
    }

    [Fact]
    public async void ListRoutes()
    {
        var controller = new ReverseProxyController(MediatorMock.Object);
        var response = new ListRoutesRequestResponse(new List<RouteConfigDto>()
        {
            new RouteConfigDto()
            {
                RouteId = "route1"
            },
            new RouteConfigDto()
            {
                RouteId = "route2"
            }
        });
        
        MediatorMock
            .Setup(it => it.Send(It.IsAny<ListRoutesRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        
        var result = await controller.ListRoutes();

        Assert.NotNull(result);
    }
    
    [Fact]
    public async void GetRouteById()
    {
        var controller = new ReverseProxyController(MediatorMock.Object);
        var response = new GetRouteByIdResponse(new RouteConfigDto()
        {
            RouteId = "route1"
        });
        
        MediatorMock
            .Setup(it => it.Send(It.IsAny<GetRouteByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        
        var result = await controller.GetRouteById("route1");

        Assert.NotNull(result);
    }

    [Fact]
    public async void AddRoute()
    {
        var controller = new ReverseProxyController(MediatorMock.Object);
        var response = new AddRouteResponse(new RouteConfigDto()
        {
            RouteId = "route1"
        });
        
        MediatorMock
            .Setup(it => it.Send(It.IsAny<AddRouteRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        
        var result = await controller.AddRoute(new RouteConfigDto()
        {
            RouteId = "route1"
        });

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async void AddRouteFailure()
    {
        var controller = new ReverseProxyController(MediatorMock.Object);
        var response = new AddRouteResponse(false, "error");
        
        MediatorMock
            .Setup(it => it.Send(It.IsAny<AddRouteRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        
        var result = await controller.AddRoute(new RouteConfigDto()
        {
            RouteId = "route1"
        });

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async void UpdateRoute()
    {
        var controller = new ReverseProxyController(MediatorMock.Object);
        var response = new RequestResponse();
        var dto = new RouteConfigDto()
        {
            RouteId = "route1"
        };
        var request = new UpdateRouteRequest(dto);
        
        MediatorMock
            .Setup(it => it.Send(It.IsAny<UpdateRouteRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        
        var result = await controller.UpdateRoute(dto);

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async void UpdateRouteFailure()
    {
        var controller = new ReverseProxyController(MediatorMock.Object);
        var response = new RequestResponse(false, "error");
        
        MediatorMock
            .Setup(it => it.Send(It.IsAny<UpdateRouteRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        
        var result = await controller.UpdateRoute(new RouteConfigDto()
        {
            RouteId = "route1"
        });

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }
}