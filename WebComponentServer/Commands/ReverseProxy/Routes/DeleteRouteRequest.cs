using System.Diagnostics.CodeAnalysis;
using MediatR;
using WebComponentServer.Commands.Responses;

namespace WebComponentServer.Commands.ReverseProxy.Routes;

[ExcludeFromCodeCoverage]
public class DeleteRouteRequest : IRequest<RequestResponse>
{
    public string Id { get; }

    public DeleteRouteRequest(string id)
    {
        Id = id;
    }
}
