using AutoMapper;
using MediatR;
using WebComponentServer.Commands.ReverseProxy.Clusters;
using WebComponentServer.Models.ClusterConfig;
using WebComponentServer.Services.ReverseProxy;

namespace WebComponentServer.Handlers.ReverseProxy.Clusters;

public class GetClusterByIdHandler : IRequestHandler<GetClusterByIdRequest, GetClusterByIdResponse>
{
    private IClustersConfigProvider _clustersConfigProvider;
    private IMapper _mapper;

    public GetClusterByIdHandler(IClustersConfigProvider clustersConfigProvider, IMapper mapper)
    {
        _clustersConfigProvider = clustersConfigProvider;
        _mapper = mapper;
    }
    
    public async Task<GetClusterByIdResponse> Handle(GetClusterByIdRequest request, CancellationToken cancellationToken)
    {
        var clusterConfig = _clustersConfigProvider.GetClusterById(request.Id);
        var clusterConfigDto = _mapper.Map<ClusterConfigDto>(clusterConfig);

        return new GetClusterByIdResponse(clusterConfigDto);
    }
}

