using Ardalis.GuardClauses;
using WebComponentServer.Guards;
using WebComponentServer.Services.ReverseProxy.Config.Cluster;
using Yarp.ReverseProxy.Configuration;

namespace WebComponentServer.Services.ReverseProxy;


public class ClustersConfigProvider : IClustersConfigProvider
{
    public readonly Dictionary<string, MutableClusterConfig> Clusters = new Dictionary<string, MutableClusterConfig>();

    public ClustersConfigProvider()
    {
    }
    
    public IReadOnlyList<ClusterConfig> ToClusterConfigList()
    {
        return Clusters.Values.Select(x => x.ToClusterConfig()).ToList();
    }

    public MutableClusterConfig CreateCluster(string id)
    {
        Guard.Against.DictionaryContainsKey(Clusters, id, $"Invalid cluster configuration : cluster with {id} already exists");

        var clusterConfig = new MutableClusterConfig(id);
        Clusters.Add(id, clusterConfig);
        return clusterConfig;
    }

    public IReadOnlyList<string> ListClusterIds()
    {
        return Clusters.Keys.ToList();
    }

    public MutableClusterConfig? GetClusterById(string id)
    {
        if (!Clusters.ContainsKey(id))
        {
            return null;
        }

        return Clusters[id];
    }

    public IReadOnlyList<MutableClusterConfig> ListClusters()
    {
        return Clusters.Values.ToList();
    }

    public void AddCluster(MutableClusterConfig clusterConfig)
    {
        Guard.Against.DictionaryContainsKey(Clusters, clusterConfig.Id, $"Cluster with id {clusterConfig.Id} already exists");

        Clusters.Add(clusterConfig.Id, clusterConfig);
    }

    public void UpdateCluster(MutableClusterConfig clusterConfig)
    {
        Guard.Against.DictionaryDoesNotContainsKey(Clusters, clusterConfig.Id,
            $"Cluster with id {clusterConfig.Id} not found");
        // if (!Clusters.ContainsKey(clusterConfig.Id))
        // {
        //     throw new ArgumentException($"Cluster with id {clusterConfig.Id} not found");
        // }

        Clusters[clusterConfig.Id] = clusterConfig;
    }
}