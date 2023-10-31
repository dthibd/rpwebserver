using AutoMapper;
using MediatR;
using WebComponentServer.Commands.ReverseProxy.Routes;
using WebComponentServer.Models.RouteConfig;
using WebComponentServer.Services.ReverseProxy;

namespace WebComponentServer.Handlers.ReverseProxy.Routes;

public class GetRouteByIdHandler : IRequestHandler<GetRouteByIdRequest, GetRouteByIdResponse>
{
    private IRoutesConfigProvider _routesConfigProvider;
    private IMapper _mapper;

    public GetRouteByIdHandler(
        IRoutesConfigProvider routesConfigProvider,
        IMapper mapper)
    {
        _routesConfigProvider = routesConfigProvider;
        _mapper = mapper;
    }
    
    public async Task<GetRouteByIdResponse> Handle(GetRouteByIdRequest request, CancellationToken cancellationToken)
    {
        var routeConfig = _routesConfigProvider.GetRouteById(request.Id);
        var routeConfigDto = _mapper.Map<RouteConfigDto>(routeConfig);

        return new GetRouteByIdResponse(routeConfigDto);
    }
}
