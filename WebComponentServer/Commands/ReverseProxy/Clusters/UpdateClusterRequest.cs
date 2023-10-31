using MediatR;
using WebComponentServer.Commands.Responses;
using WebComponentServer.Models.ClusterConfig;

namespace WebComponentServer.Commands.ReverseProxy.Clusters;


public class UpdateClusterResponse : RequestResponse<ClusterConfigDto>
{
    public UpdateClusterResponse(ClusterConfigDto value) : base(value)
    {
    }

    public UpdateClusterResponse(bool success, string error) : base(success, error)
    {
    }
}


public class UpdateClusterRequest : IRequest<UpdateClusterResponse>
{
    public ClusterConfigDto ClusterConfig { get; }

    public UpdateClusterRequest(ClusterConfigDto clusterConfig)
    {
        ClusterConfig = clusterConfig;
    }
}
