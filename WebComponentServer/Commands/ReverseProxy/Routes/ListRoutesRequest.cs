using System.Diagnostics.CodeAnalysis;
using MediatR;
using WebComponentServer.Commands.Responses;
using WebComponentServer.Models.RouteConfig;

namespace WebComponentServer.Commands.ReverseProxy.Routes;

[ExcludeFromCodeCoverage]
public class ListRoutesRequestResponse : RequestResponse<IEnumerable<RouteConfigDto>>
{
    public ListRoutesRequestResponse(IEnumerable<RouteConfigDto> value) : base(value)
    {
    }
}

[ExcludeFromCodeCoverage]
public class ListRoutesRequest : IRequest<ListRoutesRequestResponse>
{
}