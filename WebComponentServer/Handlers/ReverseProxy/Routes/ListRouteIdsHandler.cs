using MediatR;
using WebComponentServer.Commands.ReverseProxy.Routes;
using WebComponentServer.Services.ReverseProxy;

namespace WebComponentServer.Handlers.ReverseProxy.Routes;

public class ListRouteIdsHandler : IRequestHandler<ListRouteIdsRequest, ListRouteIdsRequestResponse>
{
    private IRoutesConfigProvider _routesConfigProvider;

    public ListRouteIdsHandler(IRoutesConfigProvider routesConfigProvider)
    {
        _routesConfigProvider = routesConfigProvider;
    }
    
    public async Task<ListRouteIdsRequestResponse> Handle(ListRouteIdsRequest request, CancellationToken cancellationToken)
    {
        var routesById = _routesConfigProvider.ListRouteIds();
        return new ListRouteIdsRequestResponse(routesById);
    }
}
