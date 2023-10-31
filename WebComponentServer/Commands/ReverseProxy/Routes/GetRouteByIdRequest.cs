using MediatR;
using WebComponentServer.Commands.Responses;
using WebComponentServer.Models.RouteConfig;

namespace WebComponentServer.Commands.ReverseProxy.Routes;

public class GetRouteByIdResponse : RequestResponse<RouteConfigDto>
{
    public GetRouteByIdResponse(RouteConfigDto value) : base(value)
    {
    }
}

public class GetRouteByIdRequest : IRequest<GetRouteByIdResponse>
{
    public string Id { get; }

    public GetRouteByIdRequest(string id)
    {
        Id = id;
    }
}
