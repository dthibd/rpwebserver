using System.Text.RegularExpressions;
using Yarp.ReverseProxy.Configuration;

namespace WebComponentServer.Services.ReverseProxy.Config.Route;

public class MutableRouteMatch
{
    public MutableRouteMatch()
    {
    }
    
    public string Path { get; set; }

    public void SetCatchAllPath(string path)
    {
        var matchallRegex = new Regex("^\\{(.*)\\}$");
        var parts = path.Split('/').ToList();
        var lastPart = parts.Last();

        var regexMatch = matchallRegex.Match(lastPart);
        if (!regexMatch.Success)
        {
            parts.Add("{**catch-all}");
        }

        this.Path = string.Join('/', parts);
    }

    public RouteMatch ToRouteMatch()
    {
        return new RouteMatch
        {
            Path = Path
        };
    }
}