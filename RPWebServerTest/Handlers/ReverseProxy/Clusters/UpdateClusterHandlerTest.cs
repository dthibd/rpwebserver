using AutoMapper;
using Moq;
using RPWebServer.Commands.ReverseProxy.Clusters;
using RPWebServer.Handlers.ReverseProxy.Clusters;
using RPWebServer.Models.ClusterConfig;
using RPWebServer.Services.ReverseProxy;
using RPWebServer.Services.ReverseProxy.Config.Cluster;

namespace RPWebServerTest.Handlers.ReverseProxy.Clusters;

public class UpdateClusterHandlerTest
{
    public Mock<IClustersConfigProvider> ClustersConfigProviderMock { get; } = new();

    public Mock<IMapper> MapperMock { get; } = new();

    public ClusterConfigDto ClusterCfgDto { get; } = new() { ClusterId = "testId" };

    public MutableClusterConfig MutableClusterCfg { get; } = new("testId");

    public UpdateClusterHandlerTest()
    {
        MapperMock
            .Setup(it => it.Map<MutableClusterConfig>(ClusterCfgDto))
            .Returns(MutableClusterCfg);
    }
    
    [Fact]
    public void Construct()
    {
        var handler = new UpdateClusterHandler(
            MapperMock.Object,
            ClustersConfigProviderMock.Object);

        Assert.Equal(MapperMock.Object, handler.Mapper);
        Assert.Equal(ClustersConfigProviderMock.Object, handler.ClusterConfigProvider);
    }

    [Fact]
    public async void HandleWithValidConfig()
    {
        ClustersConfigProviderMock
            .Setup(it => it.UpdateCluster(MutableClusterCfg));

        var handler = new UpdateClusterHandler(
            MapperMock.Object,
            ClustersConfigProviderMock.Object);

        var result = await handler.Handle(
            new UpdateClusterRequest(ClusterCfgDto),
            new CancellationToken());

        Assert.NotNull(result);
        Assert.True(result.Succeeded);
        Assert.Equal(ClusterCfgDto, result.Value);
    }
    
    
    [Fact]
    public async void HandleWithInvalidConfig()
    {
        ClustersConfigProviderMock
            .Setup(it => it.UpdateCluster(MutableClusterCfg))
            .Throws<ArgumentException>();

        var handler = new UpdateClusterHandler(
            MapperMock.Object,
            ClustersConfigProviderMock.Object);

        var result = await handler.Handle(
            new UpdateClusterRequest(ClusterCfgDto),
            new CancellationToken());

        Assert.NotNull(result);
        Assert.False(result.Succeeded);
    }
}