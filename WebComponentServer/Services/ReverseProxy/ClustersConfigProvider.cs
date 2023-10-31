using WebComponentServer.Services.ReverseProxy.Config.Cluster;
using Yarp.ReverseProxy.Configuration;

namespace WebComponentServer.Services.ReverseProxy;

public class ClustersConfigProvider : IClustersConfigProvider
{
    private Dictionary<string, MutableClusterConfig> _clusters = new Dictionary<string, MutableClusterConfig>();

    public ClustersConfigProvider()
    {
    }
    
    public IReadOnlyList<ClusterConfig> ToClusterConfigList()
    {
        return _clusters.Values.Select(x => x.ToClusterConfig()).ToList();
    }

    public MutableClusterConfig CreateCluster(string id)
    {
         if (_clusters.ContainsKey(id))
         {
             throw new ArgumentException($"Invalid cluser configuration : cluster with {id} already exists");
         }

         var clusterConfig = new MutableClusterConfig(id);
         _clusters.Add(id, clusterConfig);
         return clusterConfig;
    }

    public IReadOnlyList<string> ListClusterIds()
    {
        return _clusters.Keys.ToList();
    }

    public MutableClusterConfig? GetClusterById(string id)
    {
        if (!_clusters.ContainsKey(id))
        {
            return null;
        }

        return _clusters[id];
    }

    public IReadOnlyList<MutableClusterConfig> ListClusters()
    {
        return _clusters.Values.ToList();
    }

    public void AddCluster(MutableClusterConfig clusterConfig)
    {
        if (_clusters.ContainsKey(clusterConfig.Id))
        {
            throw new ArgumentException($"Cluster with id {clusterConfig.Id} already exists");
        }

        _clusters.Add(clusterConfig.Id, clusterConfig);
    }

    public void UpdateCluster(MutableClusterConfig clusterConfig)
    {
        if (!_clusters.ContainsKey(clusterConfig.Id))
        {
            throw new ArgumentException($"Cluster with id {clusterConfig.Id} not found");
        }

        _clusters[clusterConfig.Id] = clusterConfig;
    }
}

