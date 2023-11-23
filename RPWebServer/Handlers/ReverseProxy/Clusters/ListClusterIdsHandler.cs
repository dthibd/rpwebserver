using MediatR;
using RPWebServer.Commands.ReverseProxy.Clusters;
using RPWebServer.Services.ReverseProxy;

namespace RPWebServer.Handlers.ReverseProxy.Clusters;

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
