namespace WebComponentServer.Models.RouteConfig;

public class RouteConfigDto
{
    public string RouteId { get; set; }
    public string ClusterId { get; set; }
    
    public RouteMatchDto Match { get; set; }
    
    public List<Dictionary<string, string>> Transforms { get; set; }
}
