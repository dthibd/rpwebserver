using Yarp.ReverseProxy.Configuration;

namespace RPWebServer.Services.ReverseProxy;

public interface ICustomMemoryConfigFactory
{
    ICustomMemoryConfig CreateInstance(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters);
}
