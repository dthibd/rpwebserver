using WebComponentServer.Services.ReverseProxy;
using Yarp.ReverseProxy.Configuration;

namespace WebComponentServerTest.Services.ReverseProxy;

public class CustomMemoryConfigFactoryTest
{
    [Fact]
    public void CreateInstance()
    {
        var factory = new CustomMemoryConfigFactory();
        var memoryConfig = factory.CreateInstance(
            new List<RouteConfig>(),
            new List<ClusterConfig>());

        Assert.NotNull(memoryConfig);
    }
}