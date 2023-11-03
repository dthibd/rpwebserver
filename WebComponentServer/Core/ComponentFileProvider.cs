using System.IO.Abstractions;
using System.Text;
using System.Text.RegularExpressions;
using MimeTypes;

namespace WebComponentServer.Core;

public class ComponentFileProvider : IComponentProvider
{
    public string Id { get; }
    public string BaseUrl { get; }
    public string SourcePath { get; }
    
    public IFileSystem FileSystem { get; }
    
    public ComponentFileProvider(IFileSystem fileSystem, string id, string baseUrl, string sourcePath)
    {
        BaseUrl = baseUrl;
        Id = id;
        SourcePath = sourcePath;
        FileSystem = fileSystem;

        if (!BaseUrl.EndsWith("/"))
        {
            BaseUrl += "/";
        }
    }
    
    public bool MatchUrl(string url)
    {
        return url.StartsWith(BaseUrl, StringComparison.InvariantCultureIgnoreCase);
    }

    public ProviderContentResult GetContentForUrl(string url)
    {
        var regex = new Regex($"^{BaseUrl}(.*)$");
        string? content = null;

        var match = regex.Match(url);
        if (!match.Success)
        {
            throw new Exception($"content not found for {url}");
        }

        var path = Path.Combine(SourcePath, match.Groups[1].Value);
        var ext = Path.GetExtension(path);
        var mimeType = MimeTypeMap.GetMimeType(ext);
        
        var stream = FileSystem.File.OpenRead(path);

        return new ProviderContentResult()
        {
            Content = stream,
            ContentType = mimeType
        };
    }
}
