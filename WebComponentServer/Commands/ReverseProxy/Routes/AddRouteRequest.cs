using MediatR;
using WebComponentServer.Commands.Responses;
using WebComponentServer.Models.RouteConfig;

namespace WebComponentServer.Commands.ReverseProxy.Routes;

public class AddRouteResponse : RequestResponse<RouteConfigDto>
{
    public AddRouteResponse(RouteConfigDto value) : base(value)
    {
    }

    public AddRouteResponse(bool succeeded, string message) : base(succeeded, message)
    {
        
    }
}


public class AddRouteRequest : IRequest<AddRouteResponse>
{
    public RouteConfigDto RouteConfig { get; }

    public AddRouteRequest(RouteConfigDto routeConfig)
    {
        RouteConfig = routeConfig;
    }
}
