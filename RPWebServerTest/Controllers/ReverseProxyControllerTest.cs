using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RPWebServer.Commands.Responses;
using RPWebServer.Commands.ReverseProxy;
using RPWebServer.Commands.ReverseProxy.Clusters;
using RPWebServer.Commands.ReverseProxy.Routes;
using RPWebServer.Controllers;
using RPWebServer.Models.ClusterConfig;
using RPWebServer.Models.RouteConfig;

namespace RPWebServerTest.Controllers;

public class ReverseProxyControllerTest
{
    public Mock<IMediator> MediatorMock { get; } = new();

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
    public async void AddRouteWithNullDto()
    {
        var controller = new ReverseProxyController(MediatorMock.Object);
        var result = await controller.AddRoute(null);

        Assert.NotNull(result);
        Assert.IsType<BadRequestResult>(result);
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

    [Fact]
    public async void DeleteRouteSuccess()
    {
        var controller = new ReverseProxyController(MediatorMock.Object);
        var response = new RequestResponse();
        
        MediatorMock
            .Setup(it => it.Send(It.IsAny<DeleteRouteRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        
        var result = await controller.DeleteRoute("routeId");

        Assert.NotNull(result);
        Assert.IsType<OkResult>(result);
    }
    
    [Fact]
    public async void DeleteRouteFailure()
    {
        var controller = new ReverseProxyController(MediatorMock.Object);
        var response = new RequestResponse(false, "error");
        
        MediatorMock
            .Setup(it => it.Send(It.IsAny<DeleteRouteRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        
        var result = await controller.DeleteRoute("routeId");

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async void ListClusterIds()
    {
        var controller = new ReverseProxyController(MediatorMock.Object);
        var response = new ListClusterIdsResponse(new List<string>()
        {
            "cluster1",
            "cluster2"
        });
        
        MediatorMock
            .Setup(it => it.Send(It.IsAny<ListClusterIdsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        
        var result = await controller.ListClusterIds();

        Assert.NotNull(result);
    }

    [Fact]
    public async void ListClusters()
    {
        var controller = new ReverseProxyController(MediatorMock.Object);
        var response = new ListClustersResponse(new List<ClusterConfigDto>()
        {
            new ClusterConfigDto()
            {
                ClusterId = "cluster1"
            },
            new ClusterConfigDto()
            {
                ClusterId = "cluster2"
            }
        });
        
        MediatorMock
            .Setup(it => it.Send(It.IsAny<ListClustersRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        
        var result = await controller.ListClusters();

        Assert.NotNull(result);
    }
    
    [Fact]
    public async void GetClusterById()
    {
        var controller = new ReverseProxyController(MediatorMock.Object);
        var response = new GetClusterByIdResponse(new ClusterConfigDto()
        {
            ClusterId = "cluster1"
        });
        
        MediatorMock
            .Setup(it => it.Send(It.IsAny<GetClusterByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        
        var result = await controller.GetClusterById("cluster1");

        Assert.NotNull(result);
    }

    [Fact]
    public async void GetClusterByIdWithInvalidId()
    {
        var controller = new ReverseProxyController(MediatorMock.Object);
        var response = new GetClusterByIdResponse(false, "error");
        
        MediatorMock
            .Setup(it => it.Send(It.IsAny<GetClusterByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        
        var result = await controller.GetClusterById("cluster1");

        Assert.NotNull(result);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async void AddClusterSuccess()
    {
        var controller = new ReverseProxyController(MediatorMock.Object);
        var response = new AddClusterResponse(new ClusterConfigDto()
        {
            ClusterId = "cluster1"
        });
        
        MediatorMock
            .Setup(it => it.Send(It.IsAny<AddClusterRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        
        var result = await controller.AddCluster(new ClusterConfigDto()
        {
            ClusterId = "cluster1"
        });

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async void AddClusterFailure()
    {
        var controller = new ReverseProxyController(MediatorMock.Object);
        var response = new AddClusterResponse(false, "error");
        
        MediatorMock
            .Setup(it => it.Send(It.IsAny<AddClusterRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        
        var result = await controller.AddCluster(new ClusterConfigDto()
        {
            ClusterId = "cluster1"
        });

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async void UpdateClusterSuccess()
    {
        var controller = new ReverseProxyController(MediatorMock.Object);
        var dto = new ClusterConfigDto()
        {
            ClusterId = "cluster1"
        };
        var response = new UpdateClusterResponse(dto);
        
        MediatorMock
            .Setup(it => it.Send(It.IsAny<UpdateClusterRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        
        var result = await controller.UpdateCluster(dto);

        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async void UpdateClusterFailure()
    {
        var controller = new ReverseProxyController(MediatorMock.Object);
        var response = new UpdateClusterResponse(false, "error");
        
        MediatorMock
            .Setup(it => it.Send(It.IsAny<UpdateClusterRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        
        var result = await controller.UpdateCluster(new ClusterConfigDto()
        {
            ClusterId = "cluster1"
        });

        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async void Refresh()
    {
        var controller = new ReverseProxyController(MediatorMock.Object);

        MediatorMock
            .Setup(it => it.Send(It.IsAny<RefreshReverseProxyRequest>(), It.IsAny<CancellationToken>()));
        
        var result = await controller.Refresh();

        Assert.NotNull(result);
        Assert.IsType<OkResult>(result);
    }
}