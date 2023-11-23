using WebComponentServer.Services.ReverseProxy.Config.Cluster;
using Yarp.ReverseProxy.Configuration;

namespace WebComponentServer.Services.ReverseProxy;

public interface IClustersConfigProvider
{
    IReadOnlyList<ClusterConfig> ToClusterConfigList();
    MutableClusterConfig CreateCluster(string id);
    IReadOnlyList<string> ListClusterIds();
    MutableClusterConfig? GetClusterById(string id);
    IReadOnlyList<MutableClusterConfig> ListClusters();
    void AddCluster(MutableClusterConfig clusterConfig);
    void UpdateCluster(MutableClusterConfig clusterConfig);
}