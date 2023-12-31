using System.IO.Abstractions;
using Microsoft.Extensions.Options;
using RPWebServer.Configuration;
using RPWebServer.Core;

namespace RPWebServer.Services;


public class ComponentsMappingService : IComponentsMappingService
{
    public ILogger<ComponentsMappingService> Logger { get; }
    public IOptions<WebComponentsServerOptions> WebComponentsServerOptions { get; }
    public IComponentProviderFactory ProviderFactory { get; }
    public IFileSystem FileSystem { get; }
    public Dictionary<string, IComponentProvider> Providers { get; } = new Dictionary<string, IComponentProvider>();

    public ComponentsMappingService(
        ILogger<ComponentsMappingService> logger,
        IComponentProviderFactory providerFactory,
        IFileSystem fileSystem,
        IOptions<WebComponentsServerOptions> options)
    {
        Logger = logger;
        ProviderFactory = providerFactory;
        FileSystem = fileSystem;
        WebComponentsServerOptions = options;
    }

    public IComponentProvider? GetProviderForUrl(string url)
    {
        var provider = Providers.Values
            .FirstOrDefault(x => x.MatchUrl(url));
        
        return provider;
    }


    public void UpdateMapping()
    {
        var webComponents = WebComponentsServerOptions.Value?.WebComponents;

        if (webComponents == null)
        {
            Logger.LogInformation("No web components structure found in settings");
            return;
        }
        
        var currentDir = FileSystem.Directory.GetCurrentDirectory();
        var rootDir = WebComponentsServerOptions.Value?.Root;

        if (!string.IsNullOrEmpty(rootDir) && !FileSystem.Path.IsPathRooted(rootDir))
        {
            currentDir = FileSystem.Path.Combine(currentDir, rootDir);
        }

        Providers.Clear();
        
        foreach (var entry in webComponents)
        {
            var provider = CreateProviderForPath(entry.Key, currentDir, entry.Value?.FileProvider);
            if (provider != null)
            {
                Providers.Add(entry.Key, provider);
            }
        }
    }

    public IComponentProvider? CreateProviderForPath(string key, string currentDir, FileProviderOptions? fileProviderOptions)
    {
        if (fileProviderOptions == null)
        {
            Logger.LogInformation($"no file provider options for {key}");
            return null;
        }
        
        var path = FileSystem.Path.IsPathRooted(fileProviderOptions.FilePath) ? fileProviderOptions.FilePath : FileSystem.Path.Combine(currentDir, fileProviderOptions.FilePath);
        var pathExists = FileSystem.Directory.Exists(path);
        var baseUrl = fileProviderOptions.BaseUrl;

        if (pathExists)
        {
            Logger.LogInformation($"Adding provider for {key} with base url '{baseUrl}' to path '{path}' ");
            return ProviderFactory.CreateFileProvider(key, baseUrl, path);
        }

        Logger.LogInformation($"path '{path}' doesn't exists : provider {key} not created");
        return null;
    }
}
