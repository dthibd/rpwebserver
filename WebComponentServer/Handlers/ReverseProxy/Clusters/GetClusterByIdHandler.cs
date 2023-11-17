using AutoMapper;
using MediatR;
using WebComponentServer.Commands.ReverseProxy.Clusters;
using WebComponentServer.Models.ClusterConfig;
using WebComponentServer.Services.ReverseProxy;

namespace WebComponentServer.Handlers.ReverseProxy.Clusters;

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

