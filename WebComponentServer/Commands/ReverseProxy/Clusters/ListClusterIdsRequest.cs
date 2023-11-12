using System.Diagnostics.CodeAnalysis;
using MediatR;
using WebComponentServer.Commands.Responses;

namespace WebComponentServer.Commands.ReverseProxy.Clusters;

[ExcludeFromCodeCoverage]
public class ListClusterIdsResponse : RequestResponse<IReadOnlyList<string>>
{
    public IReadOnlyList<string> Ids { get; }

    public ListClusterIdsResponse(IReadOnlyList<string> value) : base(value)
    {
    }

    public ListClusterIdsResponse(bool success, string error) : base(success, error)
    {
    }
}

[ExcludeFromCodeCoverage]
public class ListClusterIdsRequest : IRequest<ListClusterIdsResponse>
{
}
