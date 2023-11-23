namespace WebComponentServer.Services.ReverseProxy.Config.Route;

public class MutableRouteTransforms
{
    public List<Dictionary<string, string>> Transforms { get; set; } = new();
    
    public void AddPathRemovePrefix(string path)
    {
        Transforms.Add(
            new Dictionary<string, string>(){ {"PathRemovePrefix", path} }
        );
    }

    public IReadOnlyList<IReadOnlyDictionary<string, string>>? ToRouteTransforms()
    {
        return Transforms;
    }
}
