using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using WebComponentServer.Models.RouteConfig;
using WebComponentServer.Services.ReverseProxy.Config.Route;

namespace WebComponentServer.AutoMapping;

[ExcludeFromCodeCoverage]
public class RouteConfigProfile : Profile
{
    public RouteConfigProfile()
    {
        CreateMap<MutableRouteConfig, RouteConfigDto>()
            .ForMember(dest => dest.Transforms, 
                opt => opt.MapFrom(src => src.Transforms.ToRouteTransforms()));
        CreateMap<MutableRouteMatch, RouteMatchDto>();
        
        CreateMap<RouteConfigDto, MutableRouteConfig>()
            .ForCtorParam("id", opt => opt.MapFrom(src => src.RouteId))
            .ForMember(dest => dest.Transforms,
                opt =>
                {
                    opt.MapFrom(src => new MutableRouteTransforms(src.Transforms));
                });
        CreateMap<RouteMatchDto, MutableRouteMatch>();
    }
}
