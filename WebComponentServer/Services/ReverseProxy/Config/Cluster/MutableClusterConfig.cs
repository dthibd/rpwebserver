using Yarp.ReverseProxy.Configuration;

namespace WebComponentServer.Services.ReverseProxy.Config.Cluster;

public class MutableClusterConfig
{
    private string _id;

    private readonly FluentMutableClusterConfig _set;

    public MutableClusterConfig(string id)
    {
        _id = id;
        _set = new FluentMutableClusterConfig(this);
    }

    public string Id => _id;
    
    public string? LoadBalancingPolicy { get; set; }

    public FluentMutableClusterConfig Set => _set;

    public Dictionary<string, MutableDestinationConfig> Destinations { get; } = new Dictionary<string, MutableDestinationConfig>();

    public ClusterConfig ToClusterConfig()
    {
        return new ClusterConfig()
        {
            ClusterId = Id,
            LoadBalancingPolicy = LoadBalancingPolicy,
            Destinations = BuildDestinationsConfigDictionary()
        };
    }

    private Dictionary<string, DestinationConfig> BuildDestinationsConfigDictionary()
    {
        var destinations = new Dictionary<string, DestinationConfig>();
        
        foreach (var entry in Destinations)
        {
            destinations.Add(entry.Key, entry.Value.ToDestinationConfig());
        }

        return destinations;
    }

    public class FluentMutableClusterConfig
    {
        private MutableClusterConfig _config;

        public FluentMutableClusterConfig(MutableClusterConfig config)
        {
            _config = config;
        }

        public FluentMutableClusterConfig LoadBalancingPolicy(string policy)
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
