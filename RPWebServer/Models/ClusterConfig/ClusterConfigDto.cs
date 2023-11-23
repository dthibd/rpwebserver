using System.Diagnostics.CodeAnalysis;

namespace RPWebServer.Models.ClusterConfig;

[ExcludeFromCodeCoverage]
public class ClusterConfigDto
{
    public string? ClusterId { get; set; }
    public string? LoadBalancingPolicy { get; set; }
    public Dictionary<string, DestinationConfigDto>? Destinations { get; set; }
}
