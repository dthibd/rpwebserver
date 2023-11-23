using MediatR;
using RPWebServer.Commands.Responses;
using RPWebServer.Commands.ReverseProxy.Routes;
using RPWebServer.Services.ReverseProxy;

namespace RPWebServer.Handlers.ReverseProxy.Routes;

public class DeleteRouteHandler : IRequestHandler<DeleteRouteRequest, RequestResponse>
{
    public IRoutesConfigProvider RoutesConfigProvider { get; }

    public DeleteRouteHandler(IRoutesConfigProvider routesConfigProvider)
    {
        RoutesConfigProvider = routesConfigProvider;
    }
    
    public async Task<RequestResponse> Handle(DeleteRouteRequest request, CancellationToken cancellationToken)
    {
        try
        {
            RoutesConfigProvider.Remove(request.Id);

            return new RequestResponse();
        }
        catch (ArgumentException ex)
        {
            return new RequestResponse(false, ex.Message);
        }
    }
}

