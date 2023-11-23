using System.Diagnostics.CodeAnalysis;
using MediatR;
using RPWebServer.Commands.Responses;

namespace RPWebServer.Commands.ReverseProxy.Clusters;

[ExcludeFromCodeCoverage]
public class ListClusterIdsResponse : RequestResponse<IReadOnlyList<string>>
{
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
