using MediatR;
using WebComponentServer.Commands.ReverseProxy.Clusters;
using WebComponentServer.Services.ReverseProxy;

namespace WebComponentServer.Handlers.ReverseProxy.Clusters;

public class ListClusterIdsHandler : IRequestHandler<ListClusterIdsRequest, ListClusterIdsResponse>
{
    public IClustersConfigProvider ClustersConfigProvider { get; }

    public ListClusterIdsHandler(IClustersConfigProvider clustersConfigProvider)
    {
        ClustersConfigProvider = clustersConfigProvider;
    }
    
    public async Task<ListClusterIdsResponse> Handle(ListClusterIdsRequest request, CancellationToken cancellationToken)
    {
        var clusterIds = ClustersConfigProvider.ListClusterIds();

        return new ListClusterIdsResponse(clusterIds);
    }
}
