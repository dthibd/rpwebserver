using AutoMapper;
using Moq;
using WebComponentServer.Commands.ReverseProxy.Clusters;
using WebComponentServer.Handlers.ReverseProxy.Clusters;
using WebComponentServer.Models.ClusterConfig;
using WebComponentServer.Services.ReverseProxy;
using WebComponentServer.Services.ReverseProxy.Config.Cluster;

namespace WebComponentServerTest.Handlers.ReverseProxy.Clusters;

public class AddClusterHandlerTest
{
    public Mock<IClustersConfigProvider> ClustersConfigProviderMock { get; set; } = new();
    
    public Mock<IMapper> MapperMock { get; set; } = new();

    public ClusterConfigDto ClusterDto {get;} = new ClusterConfigDto{ ClusterId = "test-cluster" };

    public MutableClusterConfig ClusterConfig {get;} = new MutableClusterConfig("test-cluster");

    public AddClusterHandlerTest()
    {
        MapperMock
            .Setup(m => m.Map<MutableClusterConfig>(ClusterDto))
            .Returns(ClusterConfig);
    }

    [Fact]
    public void Construct()
    {
        var handler = new AddClusterHandler(
            ClustersConfigProviderMock.Object,
            MapperMock.Object);

        Assert.Equal(ClustersConfigProviderMock.Object, handler.ClustersConfigProvider);
        Assert.Equal(MapperMock.Object, handler.Mapper);
    }

    [Fact]
    public async void HandleWithNonExistingId()
    {
        var handler = new AddClusterHandler(
            ClustersConfigProviderMock.Object,
            MapperMock.Object);

        var request = new AddClusterRequest(ClusterDto);

        var response = await handler.Handle(request, CancellationToken.None);

        Assert.NotNull(response);
        Assert.True(response.Succeeded);
    }


    [Fact]
    public async void HandleWithExistingId()
    {
        ClustersConfigProviderMock
            .Setup(m => m.AddCluster(ClusterConfig))
            .Throws(new ArgumentException("Cluster already exists"));

        var handler = new AddClusterHandler(
            ClustersConfigProviderMock.Object,
            MapperMock.Object);

        var request = new AddClusterRequest(ClusterDto);

        var response = await handler.Handle(request, CancellationToken.None);

        Assert.NotNull(response);
        Assert.False(response.Succeeded);
    }
}
