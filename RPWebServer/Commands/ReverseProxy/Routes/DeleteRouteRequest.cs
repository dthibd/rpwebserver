using System.Diagnostics.CodeAnalysis;
using MediatR;
using RPWebServer.Commands.Responses;

namespace RPWebServer.Commands.ReverseProxy.Routes;

[ExcludeFromCodeCoverage]
public class DeleteRouteRequest : IRequest<RequestResponse>
{
    public string Id { get; }

    public DeleteRouteRequest(string id)
    {
        Id = id;
    }
}
