using MediatR;
using WebComponentServer.Commands.ReverseProxy.Clusters;
using WebComponentServer.Services.ReverseProxy;

namespace WebComponentServer.Handlers.ReverseProxy.Clusters;

public class ListClusterIdsHandler : IRequestHandler<ListClusterIdsRequest, ListClusterIdsResponse>
{
    private IClustersConfigProvider _clustersConfigProvider;

    public ListClusterIdsHandler(IClustersConfigProvider clustersConfigProvider)
    {
        _clustersConfigProvider = clustersConfigProvider;
    }
    
    public async Task<ListClusterIdsResponse> Handle(ListClusterIdsRequest request, CancellationToken cancellationToken)
    {
        var clusterIds = _clustersConfigProvider.ListClusterIds();

        return new ListClusterIdsResponse(clusterIds);
    }
}
