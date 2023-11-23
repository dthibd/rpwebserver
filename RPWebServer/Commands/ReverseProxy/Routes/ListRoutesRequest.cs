using System.Diagnostics.CodeAnalysis;
using MediatR;
using RPWebServer.Commands.Responses;
using RPWebServer.Models.RouteConfig;

namespace RPWebServer.Commands.ReverseProxy.Routes;

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