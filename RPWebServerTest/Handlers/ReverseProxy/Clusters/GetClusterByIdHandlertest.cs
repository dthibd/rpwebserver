using AutoMapper;
using Moq;
using RPWebServer.Commands.ReverseProxy.Clusters;
using RPWebServer.Handlers.ReverseProxy.Clusters;
using RPWebServer.Models.ClusterConfig;
using RPWebServer.Services.ReverseProxy;
using RPWebServer.Services.ReverseProxy.Config.Cluster;

namespace RPWebServerTest.Handlers.ReverseProxy.Clusters;

public class GetClusterByIdHandlertest
{
    public Mock<IClustersConfigProvider> ClustersConfigProviderMock { get; } = new();
    public Mock<IMapper> MapperMock { get; } = new();

    [Fact]
    public void Construct()
    {
        var handler = new GetClusterByIdHandler(
            ClustersConfigProviderMock.Object,
            MapperMock.Object);

        Assert.Equal(ClustersConfigProviderMock.Object, handler.ClustersConfigProvider);
        Assert.Equal(MapperMock.Object, handler.Mapper);
    }
    
    [Fact]
    public async Task HandleWithValidId()
    {
        var clusterConfig = new MutableClusterConfig("id1");
        var clusterDto = new ClusterConfigDto()
        {
            ClusterId = "id1"
        };

        ClustersConfigProviderMock
            .Setup(it => it.GetClusterById("id1"))
            .Returns(clusterConfig);

        MapperMock
            .Setup(it => it.Map<ClusterConfigDto>(clusterConfig))
            .Returns(clusterDto);
        
        var handler = new GetClusterByIdHandler(
            ClustersConfigProviderMock.Object,
            MapperMock.Object);

        var result = await handler.Handle(
            new GetClusterByIdRequest("id1"),
            new CancellationToken());

        Assert.NotNull(result);
        Assert.True(result.Succeeded);
        Assert.NotNull(result.Value);
        Assert.Equal("id1", result.Value.ClusterId);
    }

    [Fact]
    public async void HandleWithInvalidId()
    {
        var handler = new GetClusterByIdHandler(
            ClustersConfigProviderMock.Object,
            MapperMock.Object);

        var result = await handler.Handle(
            new GetClusterByIdRequest("id1"), 
            new CancellationToken());

        Assert.NotNull(result);
        Assert.False(result.Succeeded);
    }
}