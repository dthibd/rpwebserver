using Yarp.ReverseProxy.Configuration;

namespace RPWebServer.Services.ReverseProxy;

public class CustomMemoryConfigFactory : ICustomMemoryConfigFactory
{
    public ICustomMemoryConfig CreateInstance(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
    {
        return new CustomMemoryConfig(routes, clusters);
    }
}
