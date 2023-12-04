using RPWebServer.Services.ReverseProxy.Config.Cluster;

namespace RPWebServerTest.Services.ReverseProxy.Config.Cluster;

public class LoadBalancingValueTest
{
    [Fact]
    public void FromNameWithEmptyString()
    {
        var value = LoadBalancingValue.FromName("");

        Assert.Equal(LoadBalancingValue.RoundRobin, value);
    }

    [Fact]
    public void FromNameWithValidValue()
    {
        var value = LoadBalancingValue.FromName("RoundRobin");

        Assert.Equal(LoadBalancingValue.RoundRobin, value);
    }
}
