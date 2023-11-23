using Yarp.ReverseProxy.Configuration;

namespace WebComponentServer.Services.ReverseProxy;

public interface ICustomMemoryConfigFactory
{
    ICustomMemoryConfig CreateInstance(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters);
}
