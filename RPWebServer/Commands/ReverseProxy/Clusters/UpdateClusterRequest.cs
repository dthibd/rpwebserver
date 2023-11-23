using System.Diagnostics.CodeAnalysis;
using MediatR;
using RPWebServer.Commands.Responses;
using RPWebServer.Models.ClusterConfig;

namespace RPWebServer.Commands.ReverseProxy.Clusters;

[ExcludeFromCodeCoverage]
public class UpdateClusterResponse : RequestResponse<ClusterConfigDto>
{
    public UpdateClusterResponse(ClusterConfigDto value) : base(value)
    {
    }

    public UpdateClusterResponse(bool success, string error) : base(success, error)
    {
    }
}

[ExcludeFromCodeCoverage]
public class UpdateClusterRequest : IRequest<UpdateClusterResponse>
{
    public ClusterConfigDto ClusterConfig { get; }

    public UpdateClusterRequest(ClusterConfigDto clusterConfig)
    {
        ClusterConfig = clusterConfig;
    }
}
