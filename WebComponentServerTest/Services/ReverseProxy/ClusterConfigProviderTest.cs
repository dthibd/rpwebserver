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

        provider.CreateCluster("clusterA");
        provider.CreateCluster("clusterB");
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
        provider.CreateCluster("test");

        Assert.Throws<ArgumentException>(() => provider.CreateCluster("test"));
    }

    [Fact]
    public void ListClustersIds()
    {
        var provider = new ClustersConfigProvider();
        provider.CreateCluster("testA");
        provider.CreateCluster("testB");

        var ids = provider.ListClusterIds();

        Assert.NotNull(ids);
        Assert.Equal(2, ids.Count);
        Assert.Contains("testA", ids);
        Assert.Contains("testB", ids);
    }

    [Fact]
    public void GetClusterByIdWithValidId()
    {
        var provider = new ClustersConfigProvider();
        provider.CreateCluster("testA");
        provider.CreateCluster("testB");

        var cluster = provider.GetClusterById("testA");

        Assert.NotNull(cluster);
        Assert.Equal("testA", cluster.Id);
    }

    [Fact]
    public void GetClusterByIdWithInvalidId()
    {
        var provider = new ClustersConfigProvider();
        var cluster = provider.GetClusterById("test");

        Assert.Null(cluster);
    }

    [Fact]
    public void ListClustersWithTwoElements()
    {
        var provider = new ClustersConfigProvider();
        provider.CreateCluster("testA");
        provider.CreateCluster("testB");

        var clusters = provider.ListClusters();

        Assert.NotNull(clusters);
        Assert.Equal(2, clusters.Count);
    }

    [Fact]
    public void AddCluster()
    {
        var provider = new ClustersConfigProvider();
        provider.AddCluster(new MutableClusterConfig("test"));

        Assert.Single(provider.Clusters);
        Assert.Equal("test", provider.Clusters.First().Key);
    }

    [Fact]
    public void AddClusterWithExistingId()
    {
        var provider = new ClustersConfigProvider();
        provider.CreateCluster("test");

        Assert.Throws<ArgumentException>(() => provider.AddCluster(new MutableClusterConfig("test")));
    }

    [Fact]
    public void UpdateCluster()
    {
        var provider = new ClustersConfigProvider();
        provider.CreateCluster("test");

        Assert.Null(provider.Clusters.First().Value.LoadBalancingPolicy);

        var newCluster = new MutableClusterConfig("test")
        {
            LoadBalancingPolicy = "LBP-Test"
        };

        provider.UpdateCluster(newCluster);

        var updatedCluster = provider.Clusters.First().Value;

        Assert.NotNull(updatedCluster);
        Assert.NotNull(updatedCluster.LoadBalancingPolicy);
        Assert.Equal("LBP-Test", updatedCluster.LoadBalancingPolicy);
    }

    [Fact]
    public void UpdateClusterWithInvalidId()
    {
        var provider = new ClustersConfigProvider();
        
        var newCluster = new MutableClusterConfig("test")
        {
            LoadBalancingPolicy = "LBP-Test"
        };

        Assert.Throws<ArgumentException>(() => provider.UpdateCluster(newCluster));
    }
}
