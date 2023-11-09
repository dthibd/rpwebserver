using Yarp.ReverseProxy.Configuration;

namespace WebComponentServer.Services.ReverseProxy.Config.Cluster;

public class MutableClusterConfig
{
    public MutableClusterConfig(string id)
    {
        Id = id;
        Set = new FluentMutableClusterConfig(this);
    }

    public string Id { get; }

    public LoadBalancingValue? LoadBalancingPolicy { get; set; }

    public FluentMutableClusterConfig Set { get; }

    public Dictionary<string, MutableDestinationConfig> Destinations { get; } = new Dictionary<string, MutableDestinationConfig>();

    public virtual ClusterConfig ToClusterConfig()
    {
        return new ClusterConfig()
        {
            ClusterId = Id,
            LoadBalancingPolicy = LoadBalancingPolicy?.Name,
            Destinations = BuildDestinationsConfigDictionary()
        };
    }

    public Dictionary<string, DestinationConfig> BuildDestinationsConfigDictionary()
    {
        return Destinations.
            ToDictionary(entry => entry.Key, entry => entry.Value.ToDestinationConfig());
    }

    public class FluentMutableClusterConfig
    {
        private readonly MutableClusterConfig _config;

        public FluentMutableClusterConfig(MutableClusterConfig config)
        {
            _config = config;
        }

        public FluentMutableClusterConfig LoadBalancingPolicy(LoadBalancingValue policy)
        {
            _config.LoadBalancingPolicy = policy;
            return this;
        }

        public FluentMutableClusterConfig AddDefaultDestination(string address)
        {
            _config.Destinations.Add("default", new MutableDestinationConfig(address));
            return this;
        }
    }
}
