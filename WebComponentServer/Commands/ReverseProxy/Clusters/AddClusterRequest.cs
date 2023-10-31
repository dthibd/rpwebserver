using MediatR;
using WebComponentServer.Commands.Responses;
using WebComponentServer.Models.ClusterConfig;

namespace WebComponentServer.Commands.ReverseProxy.Clusters;


public class AddClusterResponse : RequestResponse<ClusterConfigDto>
{
    public AddClusterResponse(ClusterConfigDto value) : base(value)
    {
    }

    public AddClusterResponse(bool success, string error) : base(success, error)
    {
    }
}


public class AddClusterRequest : IRequest<AddClusterResponse>
{
    public ClusterConfigDto ClusterConfig { get; }

    public AddClusterRequest(ClusterConfigDto clusterConfig)
    {
        ClusterConfig = clusterConfig;
    }
}
