using MediatR;
using WebComponentServer.Commands.Responses;
using WebComponentServer.Models.RouteConfig;

namespace WebComponentServer.Commands.ReverseProxy.Routes;

public class UpdateRouteRequest : IRequest<RequestResponse>
{
    public RouteConfigDto RouteConfig { get; }
    
    public UpdateRouteRequest(RouteConfigDto routeConfigDto)
    {
        RouteConfig = routeConfigDto;
    }
}
