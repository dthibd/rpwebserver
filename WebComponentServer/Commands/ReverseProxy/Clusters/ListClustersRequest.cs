using MediatR;
using WebComponentServer.Commands.Responses;
using WebComponentServer.Models.ClusterConfig;

namespace WebComponentServer.Commands.ReverseProxy.Clusters;

public class ListClustersResponse : RequestResponse<IEnumerable<ClusterConfigDto>>
{
    public ListClustersResponse(IEnumerable<ClusterConfigDto> value) : base(value)
    {
    }

    public ListClustersResponse(bool success, string error) : base(success, error)
    {
    }
}


public class ListClustersRequest : IRequest<ListClustersResponse>
{
}