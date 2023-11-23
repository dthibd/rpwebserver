using MediatR;
using WebComponentServer.Commands.ReverseProxy.Routes;
using WebComponentServer.Services.ReverseProxy;

namespace WebComponentServer.Handlers.ReverseProxy.Routes;

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
