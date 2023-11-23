using WebComponentServer.Services.ReverseProxy.Config.Route;

namespace WebComponentServerTest.Services.ReverseProxy.Config.Route;

public class MutableRouteTransformsTest
{
    [Fact]
    public void Constructor()
    {
        var routeTransforms = new MutableRouteTransforms();

        Assert.NotNull(routeTransforms.Transforms);
    }
    
    [Fact]
    public void ConstructorWithTransformsList()
    {
        var transformsList = new List<Dictionary<string, string>>();
        transformsList.Add(
            new Dictionary<string, string>()
            {
                {"default", "127.0.0.1"}
            }
        );
        
        var routeTransforms = new MutableRouteTransforms()
        {
            Transforms = transformsList
        };

        Assert.Single(routeTransforms.Transforms);

        var firstDict = routeTransforms.Transforms.First();

        Assert.NotNull(firstDict);
        Assert.Equal("default", firstDict.First().Key);
        Assert.Equal("127.0.0.1", firstDict.First().Value);
    }
}