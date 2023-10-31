using AutoMapper;
using MediatR;
using WebComponentServer.Commands.ReverseProxy.Clusters;
using WebComponentServer.Services.ReverseProxy;
using WebComponentServer.Services.ReverseProxy.Config.Cluster;

namespace WebComponentServer.Handlers.ReverseProxy.Clusters;

public class UpdateClusterHandler : IRequestHandler<UpdateClusterRequest, UpdateClusterResponse>
{
    private IClustersConfigProvider _clusterConfigProvider;
    private IMapper _mapper;

    public UpdateClusterHandler(IMapper mapper, IClustersConfigProvider clusterConfigProvider)
    {
        _mapper = mapper;
        _clusterConfigProvider = clusterConfigProvider;
    }
    
    public async Task<UpdateClusterResponse> Handle(UpdateClusterRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var clusterConfig = _mapper.Map<MutableClusterConfig>(request.ClusterConfig);
            _clusterConfigProvider.UpdateCluster(clusterConfig);

            return new UpdateClusterResponse(request.ClusterConfig);
        }
        catch (ArgumentException ex)
        {
            return new UpdateClusterResponse(false, ex.Message);
        }
    }
}
