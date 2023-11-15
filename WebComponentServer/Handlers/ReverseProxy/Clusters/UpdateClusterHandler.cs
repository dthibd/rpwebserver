using AutoMapper;
using MediatR;
using WebComponentServer.Commands.ReverseProxy.Clusters;
using WebComponentServer.Services.ReverseProxy;
using WebComponentServer.Services.ReverseProxy.Config.Cluster;

namespace WebComponentServer.Handlers.ReverseProxy.Clusters;

public class UpdateClusterHandler : IRequestHandler<UpdateClusterRequest, UpdateClusterResponse>
{
    public IClustersConfigProvider ClusterConfigProvider { get; }
    public IMapper Mapper { get; }

    public UpdateClusterHandler(IMapper mapper, IClustersConfigProvider clusterConfigProvider)
    {
        Mapper = mapper;
        ClusterConfigProvider = clusterConfigProvider;
    }
    
    public async Task<UpdateClusterResponse> Handle(UpdateClusterRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var clusterConfig = Mapper.Map<MutableClusterConfig>(request.ClusterConfig);
            ClusterConfigProvider.UpdateCluster(clusterConfig);

            return new UpdateClusterResponse(request.ClusterConfig);
        }
        catch (ArgumentException ex)
        {
            return new UpdateClusterResponse(false, ex.Message);
        }
    }
}
