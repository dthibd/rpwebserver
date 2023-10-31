using AutoMapper;
using WebComponentServer.Models.ClusterConfig;
using WebComponentServer.Services.ReverseProxy.Config.Cluster;

namespace WebComponentServer.AutoMapping;

public class ClusterConfigProfile : Profile
{
    public ClusterConfigProfile()
    {
        CreateMap<MutableClusterConfig, ClusterConfigDto>()
            .ForMember(dest => dest.ClusterId, 
                opt => opt.MapFrom(src => src.Id));

        CreateMap<MutableDestinationConfig, DestinationConfigDto>();

        CreateMap<ClusterConfigDto, MutableClusterConfig>()
            .ForCtorParam("id",
                opt => opt.MapFrom(src => src.ClusterId));

        CreateMap<DestinationConfigDto, MutableDestinationConfig>()
            .ForCtorParam("address",
                opt => opt.MapFrom(src => src.Address));
    }
}
