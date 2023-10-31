using Yarp.ReverseProxy.Configuration;

namespace WebComponentServer.Services.ReverseProxy.Config.Route;

public class MutableRouteConfig
{
    private string _id;
    private FluentMutableRouteConfig _set;

    public MutableRouteConfig(string id)
    {
        _id = id;
        _set = new FluentMutableRouteConfig(this);
    }

    public FluentMutableRouteConfig Set => _set;
    
    public string RouteId => _id;

    public string? ClusterId { get; set; }

    public MutableRouteMatch Match { get; set; } = new MutableRouteMatch();

    public MutableRouteTransforms Transforms { get; set; } = new MutableRouteTransforms();
    
    public RouteConfig ToRouteConfig()
    {
        return new RouteConfig
        {
            RouteId = RouteId,
            ClusterId = ClusterId,
            Match = Match.ToRouteMatch(),
            Transforms = Transforms.ToRouteTransforms()
        };
    }


    public class FluentMutableRouteConfig
    {
        private MutableRouteConfig _config;

        public FluentMutableRouteConfig(MutableRouteConfig config)
        {
            _config = config;
        }

        public FluentMutableRouteConfig ClusterId(string clusterId)
        {
            _config.ClusterId = clusterId;
            return this;
        }

        public FluentMutableRouteConfig Match_CatchAllPath(string match)
        {
            _config.Match.SetCatchAllPath(match);
            return this;
        }

        public FluentMutableRouteConfig Transforms_PathRemovePrefix(string path)
        {
            _config.Transforms.AddPathRemovePrefix(path);
            return this;
        }
    }
}
