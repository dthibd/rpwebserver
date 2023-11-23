using System.Diagnostics.CodeAnalysis;
using MediatR;
using RPWebServer.Commands.Responses;
using RPWebServer.Models.ClusterConfig;

namespace RPWebServer.Commands.ReverseProxy.Clusters;

[ExcludeFromCodeCoverage]
public class ListClustersResponse : RequestResponse<IEnumerable<ClusterConfigDto>>
{
    public ListClustersResponse(IEnumerable<ClusterConfigDto> value) : base(value)
    {
    }

    public ListClustersResponse(bool success, string error) : base(success, error)
    {
    }
}

[ExcludeFromCodeCoverage]
public class ListClustersRequest : IRequest<ListClustersResponse>
{
}