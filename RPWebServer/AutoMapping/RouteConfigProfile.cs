using AutoMapper;
using RPWebServer.Models.RouteConfig;
using RPWebServer.Services.ReverseProxy.Config.Route;

namespace RPWebServer.AutoMapping;


public class RouteConfigProfile : Profile
{
    public RouteConfigProfile()
    {
        CreateMap<MutableRouteConfig, RouteConfigDto>();
        CreateMap<MutableRouteMatch, RouteMatchDto>();
        CreateMap<MutableRouteTransforms, List<Dictionary<string, string>>>()
            .ConvertUsing((source, destination, context) => source.Transforms);

        CreateMap<RouteConfigDto, MutableRouteConfig>()
            .ForCtorParam("id", opt => opt.MapFrom(src => src.RouteId));
        CreateMap<RouteMatchDto, MutableRouteMatch>();
        CreateMap<List<Dictionary<string, string>>, MutableRouteTransforms>()
            .ConvertUsing((source, destination, context) => new MutableRouteTransforms()
            {
                Transforms = source
            });
    }
}
