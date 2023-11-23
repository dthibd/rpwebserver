using AutoMapper;
using MediatR;
using WebComponentServer.Commands.ReverseProxy.Routes;
using WebComponentServer.Services.ReverseProxy;
using WebComponentServer.Services.ReverseProxy.Config.Route;

namespace WebComponentServer.Handlers.ReverseProxy.Routes;

public class AddRouteHandler : IRequestHandler<AddRouteRequest, AddRouteResponse>
{
    public IRoutesConfigProvider RoutesConfigProvider { get; }
    public IMapper Mapper { get; }

    public AddRouteHandler(
        IRoutesConfigProvider routesConfigProvider,
        IMapper mapper)
    {
        RoutesConfigProvider = routesConfigProvider;
        Mapper = mapper;
    }
    
    public async Task<AddRouteResponse> Handle(AddRouteRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var mutableRouteConfig = Mapper.Map<MutableRouteConfig>(request.RouteConfig);

            RoutesConfigProvider.Add(mutableRouteConfig);

            return new AddRouteResponse(request.RouteConfig);
        }
        catch (ArgumentException ex)
        {
            return new AddRouteResponse(false, ex.Message);
        }
    }
}