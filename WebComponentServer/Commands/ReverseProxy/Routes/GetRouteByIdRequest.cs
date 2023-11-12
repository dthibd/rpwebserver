using System.Diagnostics.CodeAnalysis;
using MediatR;
using WebComponentServer.Commands.Responses;
using WebComponentServer.Models.RouteConfig;

namespace WebComponentServer.Commands.ReverseProxy.Routes;

[ExcludeFromCodeCoverage]
public class GetRouteByIdResponse : RequestResponse<RouteConfigDto>
{
    public GetRouteByIdResponse(RouteConfigDto value) : base(value)
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
