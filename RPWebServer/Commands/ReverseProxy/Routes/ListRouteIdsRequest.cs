using System.Diagnostics.CodeAnalysis;
using MediatR;
using RPWebServer.Commands.Responses;

namespace RPWebServer.Commands.ReverseProxy.Routes;

[ExcludeFromCodeCoverage]
public class ListRouteIdsRequestResponse : RequestResponse<IReadOnlyList<string>>
{
    public ListRouteIdsRequestResponse(IReadOnlyList<string> value) : base(value)
    {
    }
}

[ExcludeFromCodeCoverage]
public class ListRouteIdsRequest : IRequest<ListRouteIdsRequestResponse>
{
    
}