namespace WebComponentServer.Core;

public struct ProviderContentResult
{
    public Stream? Content;
    public string ContentType;
}

public interface IComponentProvider
{
    string Id { get; }
    bool MatchUrl(string url);
    ProviderContentResult GetContentForUrl(string url);
}