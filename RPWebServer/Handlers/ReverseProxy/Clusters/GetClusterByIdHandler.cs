using AutoMapper;
using MediatR;
using RPWebServer.Commands.ReverseProxy.Clusters;
using RPWebServer.Models.ClusterConfig;
using RPWebServer.Services.ReverseProxy;

namespace RPWebServer.Handlers.ReverseProxy.Clusters;

public class GetClusterByIdHandler : IRequestHandler<GetClusterByIdRequest, GetClusterByIdResponse>
{
    public IClustersConfigProvider ClustersConfigProvider { get; }
    public IMapper Mapper { get; }

    public GetClusterByIdHandler(IClustersConfigProvider clustersConfigProvider, IMapper mapper)
    {
        ClustersConfigProvider = clustersConfigProvider;
        Mapper = mapper;
    }
    
    public async Task<GetClusterByIdResponse> Handle(GetClusterByIdRequest request, CancellationToken cancellationToken)
    {
        var clusterConfig = ClustersConfigProvider.GetClusterById(request.Id);

        if (clusterConfig == null)
        {
            return new GetClusterByIdResponse(false, "cluster not found");
        }
        
        var clusterConfigDto = Mapper.Map<ClusterConfigDto>(clusterConfig);

        return new GetClusterByIdResponse(clusterConfigDto);
    }
}

