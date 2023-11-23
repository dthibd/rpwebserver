using System.Diagnostics.CodeAnalysis;
using MediatR;
using RPWebServer.Commands.Responses;
using RPWebServer.Models.RouteConfig;

namespace RPWebServer.Commands.ReverseProxy.Routes;

[ExcludeFromCodeCoverage]
public class AddRouteResponse : RequestResponse<RouteConfigDto>
{
    public AddRouteResponse(RouteConfigDto value) : base(value)
    {
    }

    public AddRouteResponse(bool succeeded, string message) : base(succeeded, message)
    {
        
    }
}

[ExcludeFromCodeCoverage]
public class AddRouteRequest : IRequest<AddRouteResponse>
{
    public RouteConfigDto RouteConfig { get; }

    public AddRouteRequest(RouteConfigDto routeConfig)
    {
        RouteConfig = routeConfig;
    }
}
