using MediatR;
using WebComponentServer.Commands.Responses;
using WebComponentServer.Models.ClusterConfig;

namespace WebComponentServer.Commands.ReverseProxy.Clusters;

public class GetClusterByIdResponse : RequestResponse<ClusterConfigDto>
{
    public GetClusterByIdResponse(ClusterConfigDto value) : base(value)
    {
    }

    public GetClusterByIdResponse(bool success, string error) : base(success, error)
    {
    }
}

public class GetClusterByIdRequest : IRequest<GetClusterByIdResponse>
{
    public string Id { get; }

    public GetClusterByIdRequest(string id)
    {
        Id = id;
    }
}

