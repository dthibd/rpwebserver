namespace WebComponentServer.Configuration;

public sealed class WebComponentsServerOptions
{
    public string? Root { get; set; }
    public Dictionary<string, WebComponentOptions>?      WebComponents { get; set; }
}
