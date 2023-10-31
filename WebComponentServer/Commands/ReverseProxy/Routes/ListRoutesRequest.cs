using MediatR;
using WebComponentServer.Commands.Responses;
using WebComponentServer.Models.RouteConfig;

namespace WebComponentServer.Commands.ReverseProxy.Routes;

public class ListRoutesRequestResponse : RequestResponse<IEnumerable<RouteConfigDto>>
{
    public ListRoutesRequestResponse(IEnumerable<RouteConfigDto> value) : base(value)
    {
    }
}

public class ListRoutesRequest : IRequest<ListRoutesRequestResponse>
{
}