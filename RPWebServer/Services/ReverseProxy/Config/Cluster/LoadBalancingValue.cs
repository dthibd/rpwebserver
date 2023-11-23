using Ardalis.SmartEnum;
using Yarp.ReverseProxy.LoadBalancing;

namespace RPWebServer.Services.ReverseProxy.Config.Cluster;

public class LoadBalancingValue : SmartEnum<LoadBalancingValue>
{
    public static readonly LoadBalancingValue RoundRobin = new LoadBalancingValue(LoadBalancingPolicies.RoundRobin, 1);
    
    private LoadBalancingValue(string name, int value) : base(name, value)
    {
    }
}
