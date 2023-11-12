using AutoMapper;
using MediatR;
using WebComponentServer.Commands.ReverseProxy.Routes;
using WebComponentServer.Models.RouteConfig;
using WebComponentServer.Services.ReverseProxy;

namespace WebComponentServer.Handlers.ReverseProxy.Routes;

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
