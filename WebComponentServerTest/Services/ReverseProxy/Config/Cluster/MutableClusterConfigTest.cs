using WebComponentServer.Services.ReverseProxy.Config.Cluster;

namespace WebComponentServerTest.Services.ReverseProxy.Config.Cluster;

public class MutableClusterConfigTest
{
    [Fact]
    public void ValidConstruction()
    {
        var config = new MutableClusterConfig("test");

        Assert.Equal("test", config.Id);
        Assert.NotNull(config.Set);
    }

    [Fact]
    public void ToClusterConfig()
    {
        var mutableConfig = new MutableClusterConfig("test");
        mutableConfig.Set
            .LoadBalancingPolicy(LoadBalancingValue.RoundRobin)
            .AddDefaultDestination("127.0.0.1");

        var config = mutableConfig.ToClusterConfig();

        Assert.NotNull(config);
        Assert.Equal("test", config.ClusterId);
        Assert.Equal(LoadBalancingValue.RoundRobin.Name, config.LoadBalancingPolicy);
        Assert.NotNull(config.Destinations);
        Assert.Single(config.Destinations);
    }

    [Fact]
    public void BuildDestinationsConfigDictionary()
    {
        var mutableConfig = new MutableClusterConfig("test");
        mutableConfig.Set
            .LoadBalancingPolicy(LoadBalancingValue.RoundRobin)
            .AddDefaultDestination("127.0.0.1");

        var destinations = mutableConfig.BuildDestinationsConfigDictionary();

        Assert.NotNull(destinations);
        Assert.Single(destinations);
        Assert.Equal("127.0.0.1", destinations.First().Value.Address);
        Assert.Equal("default", destinations.First().Key);
    }
    
}
