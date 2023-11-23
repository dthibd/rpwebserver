using AutoMapper;
using MediatR;
using RPWebServer.Commands.ReverseProxy.Routes;
using RPWebServer.Models.RouteConfig;
using RPWebServer.Services.ReverseProxy;

namespace RPWebServer.Handlers.ReverseProxy.Routes;

public class ListRoutesHandler : IRequestHandler<ListRoutesRequest, ListRoutesRequestResponse>
{
    public IRoutesConfigProvider RoutesConfigProvider { get; }
    public IMapper Mapper { get; }

    public ListRoutesHandler(
        IRoutesConfigProvider routesConfigProvider,
        IMapper mapper )
    {
        RoutesConfigProvider = routesConfigProvider;
        Mapper = mapper;
    }

    
    public async Task<ListRoutesRequestResponse> Handle(ListRoutesRequest request, CancellationToken cancellationToken)
    {
        var routes = RoutesConfigProvider.ListRoutes();
        var routesDto = routes.Select(x => Mapper.Map<RouteConfigDto>(x));

        return new ListRoutesRequestResponse(routesDto);
    }
}
