using MediatR;
using WebComponentServer.Commands.Responses;

namespace WebComponentServer.Commands.ReverseProxy.Routes;

public class DeleteRouteRequest : IRequest<RequestResponse>
{
    public string Id { get; }

    public DeleteRouteRequest(string id)
    {
        Id = id;
    }
}
