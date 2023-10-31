using AutoMapper;
using MediatR;
using WebComponentServer.Commands.ReverseProxy.Clusters;
using WebComponentServer.Services.ReverseProxy;
using WebComponentServer.Services.ReverseProxy.Config.Cluster;

namespace WebComponentServer.Handlers.ReverseProxy.Clusters;

public class AddClusterHandler : IRequestHandler<AddClusterRequest, AddClusterResponse>
{
    private IClustersConfigProvider _clustersConfigProvider;
    private IMapper _mapper;

    public AddClusterHandler(IClustersConfigProvider clustersConfigProvider, IMapper mapper)
    {
        _clustersConfigProvider = clustersConfigProvider;
        _mapper = mapper;
    }
    
    public async Task<AddClusterResponse> Handle(AddClusterRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var clusterConfig = _mapper.Map<MutableClusterConfig>(request.ClusterConfig);

            _clustersConfigProvider.AddCluster(clusterConfig);

            return new AddClusterResponse(request.ClusterConfig);
        }
        catch (ArgumentException ex)
        {
            return new AddClusterResponse(false, ex.Message);
        }
    }
}
