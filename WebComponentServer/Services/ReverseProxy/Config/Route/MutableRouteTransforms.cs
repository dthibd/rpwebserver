namespace WebComponentServer.Services.ReverseProxy.Config.Route;

public class MutableRouteTransforms
{
    public List<Dictionary<string, string>> Transforms { get; }

    public MutableRouteTransforms()
    {
        Transforms = new List<Dictionary<string, string>>();
    }

    public MutableRouteTransforms(List<Dictionary<string, string>> transforms)
    {
        Transforms = transforms;
    }
    
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
