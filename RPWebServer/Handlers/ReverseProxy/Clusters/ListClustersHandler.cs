using AutoMapper;
using MediatR;
using WebComponentServer.Commands.ReverseProxy.Clusters;
using WebComponentServer.Models.ClusterConfig;
using WebComponentServer.Services.ReverseProxy;

namespace WebComponentServer.Handlers.ReverseProxy.Clusters;

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
