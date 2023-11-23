using AutoMapper;
using MediatR;
using WebComponentServer.Commands.ReverseProxy.Clusters;
using WebComponentServer.Services.ReverseProxy;
using WebComponentServer.Services.ReverseProxy.Config.Cluster;

namespace WebComponentServer.Handlers.ReverseProxy.Clusters;

public class AddClusterHandler : IRequestHandler<AddClusterRequest, AddClusterResponse>
{
    public IClustersConfigProvider ClustersConfigProvider { get; }
    public IMapper Mapper { get; }

    public AddClusterHandler(IClustersConfigProvider clustersConfigProvider, IMapper mapper)
    {
        ClustersConfigProvider = clustersConfigProvider;
        Mapper = mapper;
    }
    
    public async Task<AddClusterResponse> Handle(AddClusterRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var clusterConfig = Mapper.Map<MutableClusterConfig>(request.ClusterConfig);

            ClustersConfigProvider.AddCluster(clusterConfig);

            return new AddClusterResponse(request.ClusterConfig);
        }
        catch (ArgumentException ex)
        {
            return new AddClusterResponse(false, ex.Message);
        }
    }
}
