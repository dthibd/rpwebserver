using AutoMapper;
using MediatR;
using RPWebServer.Commands.ReverseProxy.Routes;
using RPWebServer.Services.ReverseProxy;
using RPWebServer.Services.ReverseProxy.Config.Route;

namespace RPWebServer.Handlers.ReverseProxy.Routes;

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