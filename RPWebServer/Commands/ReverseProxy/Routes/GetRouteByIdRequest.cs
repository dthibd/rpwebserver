using System.Diagnostics.CodeAnalysis;
using MediatR;
using RPWebServer.Commands.Responses;
using RPWebServer.Models.RouteConfig;

namespace RPWebServer.Commands.ReverseProxy.Routes;

[ExcludeFromCodeCoverage]
public class GetRouteByIdResponse : RequestResponse<RouteConfigDto?>
{
    public GetRouteByIdResponse(RouteConfigDto? value) : base(value)
    {
    }
}

[ExcludeFromCodeCoverage]
public class GetRouteByIdRequest : IRequest<GetRouteByIdResponse>
{
    public string Id { get; }

    public GetRouteByIdRequest(string id)
    {
        Id = id;
    }
}
