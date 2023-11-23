using System.Diagnostics.CodeAnalysis;
using MediatR;
using RPWebServer.Commands.Responses;
using RPWebServer.Models.RouteConfig;

namespace RPWebServer.Commands.ReverseProxy.Routes;

[ExcludeFromCodeCoverage]
public class UpdateRouteRequest : IRequest<RequestResponse>
{
    public RouteConfigDto RouteConfig { get; }
    
    public UpdateRouteRequest(RouteConfigDto routeConfigDto)
    {
        RouteConfig = routeConfigDto;
    }
}
