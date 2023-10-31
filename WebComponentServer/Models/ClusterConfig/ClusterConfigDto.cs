namespace WebComponentServer.Models.ClusterConfig;

public class ClusterConfigDto
{
    public string ClusterId { get; set; }
    public string LoadBalancingPolicy { get; set; }
    public Dictionary<string, DestinationConfigDto> Destinations { get; set; }
}
