using MediatR;
using WebComponentServer.Commands.Responses;

namespace WebComponentServer.Commands.ReverseProxy.Routes;

public class ListRouteIdsRequestResponse : RequestResponse<IReadOnlyList<string>>
{
    public ListRouteIdsRequestResponse(IReadOnlyList<string> value) : base(value)
    {
    }
}

public class ListRouteIdsRequest : IRequest<ListRouteIdsRequestResponse>
{
    
}