using System.Diagnostics.CodeAnalysis;
using MediatR;
using RPWebServer.Commands.Responses;
using RPWebServer.Models.ClusterConfig;

namespace RPWebServer.Commands.ReverseProxy.Clusters;

[ExcludeFromCodeCoverage]
public class AddClusterResponse : RequestResponse<ClusterConfigDto>
{
    public AddClusterResponse(ClusterConfigDto value) : base(value)
    {
    }

    public AddClusterResponse(bool success, string error) : base(success, error)
    {
    }
}


[ExcludeFromCodeCoverage]
public class AddClusterRequest : IRequest<AddClusterResponse>
{
    public ClusterConfigDto ClusterConfig { get; }

    public AddClusterRequest(ClusterConfigDto clusterConfig)
    {
        ClusterConfig = clusterConfig;
    }
}
