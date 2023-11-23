using AutoMapper;
using Moq;
using WebComponentServer.Commands.ReverseProxy.Clusters;
using WebComponentServer.Handlers.ReverseProxy.Clusters;
using WebComponentServer.Models.ClusterConfig;
using WebComponentServer.Services.ReverseProxy;
using WebComponentServer.Services.ReverseProxy.Config.Cluster;
using Yarp.ReverseProxy.Configuration;

namespace WebComponentServerTest.Handlers.ReverseProxy.Clusters;

public class ListClustersHandlerTest
{
    public Mock<IClustersConfigProvider> ClustersConfigProviderMock { get; } = new();

    public Mock<IMapper> MapperMock { get; } = new();

    public ClusterConfigDto[] ClustersDto { get; } = new[]
    {
        new ClusterConfigDto()
        {
            ClusterId = "cluster1"
        },
        new ClusterConfigDto()
        {
            ClusterId = "cluster2"
        }
    };

    public MutableClusterConfig[] MutableClustersCfg { get; } = new[]
    {
        new MutableClusterConfig("cluster1"),
        new MutableClusterConfig("cluster2")
    };

    public ListClustersHandlerTest()
    {
        MapperMock
            .Setup(it => it.Map<ClusterConfigDto>(It.IsAny<MutableClusterConfig>()))
            .Returns((MutableClusterConfig x) => ClustersDto.First(y => y .ClusterId == x.Id));
            // {
            //     return ClustersDto.First(c => c.ClusterId == x.ClusterId);
            // });

        ClustersConfigProviderMock
            .Setup(it => it.ListClusters())
            .Returns(MutableClustersCfg.ToList());
    }

    [Fact]
    public void Construct()
    {
        var handler = new ListClustersHandler(ClustersConfigProviderMock.Object, MapperMock.Object);

        Assert.Equal(MapperMock.Object, handler.Mapper);
        Assert.Equal(ClustersConfigProviderMock.Object, handler.ClustersConfigProvider);
    }

    [Fact]
    public async void Handle()
    {
        var handler = new ListClustersHandler(ClustersConfigProviderMock.Object, MapperMock.Object);

        var result = await handler.Handle(
            new ListClustersRequest(),
            new CancellationToken());

        Assert.NotNull(result);
        Assert.True(result.Succeeded);
        Assert.NotNull(result.Value);

        var clusters = result.Value;

        Assert.Equal(2, clusters.Count());
        Assert.Contains(ClustersDto[0], clusters);
        Assert.Contains(ClustersDto[1], clusters);
    }
}