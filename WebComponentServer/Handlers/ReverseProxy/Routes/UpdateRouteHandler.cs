using AutoMapper;
using MediatR;
using WebComponentServer.Commands.Responses;
using WebComponentServer.Commands.ReverseProxy.Routes;
using WebComponentServer.Services.ReverseProxy;
using WebComponentServer.Services.ReverseProxy.Config.Route;

namespace WebComponentServer.Handlers.ReverseProxy.Routes;

public class UpdateRouteHandler : IRequestHandler<UpdateRouteRequest, RequestResponse>
{
    public IRoutesConfigProvider RoutesConfigProvider { get; }
    public IMapper Mapper { get; }
    
    public UpdateRouteHandler(
        IRoutesConfigProvider routesConfigProvider,
        IMapper mapper )
    {
        RoutesConfigProvider = routesConfigProvider;
        Mapper = mapper;
    }
    
    public async Task<RequestResponse> Handle(UpdateRouteRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var mutableRouteConfig = Mapper.Map<MutableRouteConfig>(request.RouteConfig);

            RoutesConfigProvider.Update(mutableRouteConfig);

            return new RequestResponse();
        }
        catch (ArgumentException ex)
        {
            return new RequestResponse(false, ex.Message);
        }
    }
}
