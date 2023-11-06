using WebComponentServer.Services.ReverseProxy.Config.Route;
using Yarp.ReverseProxy.Configuration;

namespace WebComponentServer.Services.ReverseProxy;

public class RoutesConfigProvider : IRoutesConfigProvider
{
    public readonly Dictionary<string, MutableRouteConfig> Routes = new Dictionary<string, MutableRouteConfig>();

    public RoutesConfigProvider()
    {
        
    }

    public IReadOnlyList<RouteConfig> ToRouteConfigList()
    {
        return Routes.Values
                .Select(x => x.ToRouteConfig())
                .ToList();
    }

    public MutableRouteConfig Create(string id)
    {
        if (Routes.ContainsKey(id))
        {
            throw new ArgumentException($"RouteConfig with id {id} already exists");
        }
        
        var route = new MutableRouteConfig(id);
        Routes.Add(id, route);
        
        return route;
    }

    public void Add(MutableRouteConfig routeConfig)
    {
        if (Routes.ContainsKey(routeConfig.RouteId))
        {
            throw new ArgumentException($"RouteConfig with id {routeConfig.RouteId} already exists");
        }

        Routes.Add(routeConfig.RouteId, routeConfig);
    }

    public void Update(MutableRouteConfig routeConfig)
    {
        if (!Routes.ContainsKey(routeConfig.RouteId))
        {
            throw new ArgumentException($"RouteConfig with id {routeConfig.RouteId} not found");
        }

        Routes[routeConfig.RouteId] = routeConfig;
    }

    public void Remove(string id)
    {
        if (!Routes.ContainsKey(id))
        {
            throw new ArgumentException($"RouteConfig with id {id} not found");
        }

        Routes.Remove(id);
    }

    public IReadOnlyList<string> ListRouteIds()
    {
        return Routes.Keys.ToList();
    }

    public MutableRouteConfig? GetRouteById(string id)
    {
        if (!Routes.ContainsKey(id))
        {
            return null;
        }

        return Routes[id];
    }

    public IReadOnlyList<MutableRouteConfig> ListRoutes()
    {
        return Routes.Values.ToList();
    }
}
