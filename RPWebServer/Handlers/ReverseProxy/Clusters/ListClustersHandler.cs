using AutoMapper;
using MediatR;
using RPWebServer.Commands.ReverseProxy.Clusters;
using RPWebServer.Models.ClusterConfig;
using RPWebServer.Services.ReverseProxy;

namespace RPWebServer.Handlers.ReverseProxy.Clusters;

public class ListClustersHandler : IRequestHandler<ListClustersRequest, ListClustersResponse>
{
    public IClustersConfigProvider ClustersConfigProvider { get; }
    public IMapper Mapper { get; }

    public ListClustersHandler(IClustersConfigProvider clustersConfigProvider, IMapper mapper)
    {
        ClustersConfigProvider = clustersConfigProvider;
        Mapper = mapper;
    }
    
    public async Task<ListClustersResponse> Handle(ListClustersRequest request, CancellationToken cancellationToken)
    {
        var clusters = ClustersConfigProvider.ListClusters();

        var clustersDto = clusters.Select(x => Mapper.Map<ClusterConfigDto>(x));

        return new ListClustersResponse(clustersDto);
    }
}
