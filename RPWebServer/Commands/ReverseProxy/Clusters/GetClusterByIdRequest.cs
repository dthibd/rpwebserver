using System.Diagnostics.CodeAnalysis;
using MediatR;
using RPWebServer.Commands.Responses;
using RPWebServer.Models.ClusterConfig;

namespace RPWebServer.Commands.ReverseProxy.Clusters;

[ExcludeFromCodeCoverage]
public class GetClusterByIdResponse : RequestResponse<ClusterConfigDto>
{
    public GetClusterByIdResponse(ClusterConfigDto value) : base(value)
    {
    }

    public GetClusterByIdResponse(bool success, string error) : base(success, error)
    {
    }
}

[ExcludeFromCodeCoverage]
public class GetClusterByIdRequest : IRequest<GetClusterByIdResponse>
{
    public string Id { get; }

    public GetClusterByIdRequest(string id)
    {
        Id = id;
    }
}

