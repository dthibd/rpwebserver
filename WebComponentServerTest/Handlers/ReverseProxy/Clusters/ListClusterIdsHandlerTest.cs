using Moq;
using WebComponentServer.Commands.ReverseProxy.Clusters;
using WebComponentServer.Handlers.ReverseProxy.Clusters;
using WebComponentServer.Services.ReverseProxy;

namespace WebComponentServerTest.Handlers.ReverseProxy.Clusters;

public class ListClusterIdsHandlerTest
{
    public Mock<IClustersConfigProvider> ClustersConfigProviderMock { get; set; } = new();

    public ListClusterIdsHandlerTest()
    {
        ClustersConfigProviderMock
            .Setup(it => it.ListClusterIds())
            .Returns(new List<string> { "id1", "id2" });
    }

    [Fact]
    public void Construction()
    {
        var handler = new ListClusterIdsHandler(ClustersConfigProviderMock.Object);

        Assert.Equal(ClustersConfigProviderMock.Object, handler.ClustersConfigProvider);
    }

    [Fact]
    public async void HandleReturnIds()
    {
        var handler = new ListClusterIdsHandler(ClustersConfigProviderMock.Object);

        var result = await handler.Handle(new ListClusterIdsRequest(), new CancellationToken());

        Assert.NotNull(result);
        Assert.True(result.Succeeded);
        Assert.NotNull(result.Value);

        var idsList = result.Value;

        Assert.Equal(2, idsList.Count);
        Assert.Contains("id1", idsList);
        Assert.Contains("id2", idsList);
    }
}