using MediatR;
using RPWebServer.Commands.ReverseProxy.Routes;
using RPWebServer.Services.ReverseProxy;

namespace RPWebServer.Handlers.ReverseProxy.Routes;

public class ListRouteIdsHandler : IRequestHandler<ListRouteIdsRequest, ListRouteIdsRequestResponse>
{
    public IRoutesConfigProvider RoutesConfigProvider { get; }

    public ListRouteIdsHandler(IRoutesConfigProvider routesConfigProvider)
    {
        RoutesConfigProvider = routesConfigProvider;
    }
    
    public async Task<ListRouteIdsRequestResponse> Handle(ListRouteIdsRequest request, CancellationToken cancellationToken)
    {
        var routesById = RoutesConfigProvider.ListRouteIds();
        return new ListRouteIdsRequestResponse(routesById);
    }
}
