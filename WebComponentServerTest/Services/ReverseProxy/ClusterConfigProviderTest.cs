using Moq;
using WebComponentServer.Services.ReverseProxy;
using WebComponentServer.Services.ReverseProxy.Config.Cluster;
using Yarp.ReverseProxy.Configuration;

namespace WebComponentServerTest.Services.ReverseProxy;

public class ClusterConfigProviderTest
{
    [Fact]
    public void ToClusterConfigList()
    {
        var provider = new ClustersConfigProvider();
        var clusterConfigMock = new Mock<MutableClusterConfig>("clusterC");

        clusterConfigMock
            .Setup(it => it.ToClusterConfig())
            .Returns(new ClusterConfig()
            {
                ClusterId = "clusterC"
            });

        provider.Clusters.Add("clusterA", new MutableClusterConfig("clusterA"));
        provider.Clusters.Add("clusterB", new MutableClusterConfig("clusterB"));
        provider.Clusters.Add("clusterC", clusterConfigMock.Object);

        var clusters = provider.ToClusterConfigList();

        Assert.Equal(3, clusters.Count);
        clusterConfigMock.Verify(it => it.ToClusterConfig(), Times.Once);
    }

    [Fact]
    public void CreateCluster()
    {
        var provider = new ClustersConfigProvider();

        var cluster = provider.CreateCluster("test");

        Assert.NotNull(cluster);
        Assert.Single(provider.Clusters);
    }

    [Fact]
    public void CreateClusterWithExistingId()
    {
        var provider = new ClustersConfigProvider();
        provider.Clusters.Add("test", new MutableClusterConfig("test"));

        Assert.Throws<ArgumentException>(() => provider.CreateCluster("test"));
    }
}