using Microsoft.Extensions.Options;
using WebComponentServer.Configuration;
using WebComponentServer.Core;

namespace WebComponentServer.Services;


public class ComponentsMappingService : IComponentsMappingService
{
    private ILogger<ComponentsMappingService> _logger;
    private IOptions<WebComponentsServerOptions> _webComponentsServerOptions;
    private IComponentProviderFactory _providerFactory;

    private Dictionary<string, IComponentProvider> _providers = new Dictionary<string, IComponentProvider>();

    public ComponentsMappingService(
        ILogger<ComponentsMappingService> logger,
        IComponentProviderFactory providerFactory,
        IOptions<WebComponentsServerOptions> options)
    {
        _logger = logger;
        _providerFactory = providerFactory;
        _webComponentsServerOptions = options;

        CreateMapping();
    }

    public IComponentProvider? GetProviderForUrl(string url)
    {
        var provider = _providers.Values
            .FirstOrDefault(x => x.MatchUrl(url));
        
        return provider;
    }


    private void CreateMapping()
    {
        var webComponents = _webComponentsServerOptions.Value?.WebComponents;

        if (webComponents == null)
        {
            _logger.LogInformation("No web components found in settings");
            return;
        }
        
        var currentDir = Directory.GetCurrentDirectory();
        var rootDir = _webComponentsServerOptions.Value?.Root;

        if (!string.IsNullOrEmpty(rootDir))
        {
            currentDir = Path.Combine(currentDir, rootDir);
        }
        
        foreach (var entry in webComponents)
        {
            var provider = CreateProviderForPath(entry.Key, currentDir, entry.Value?.FileProvider);
            if (provider != null)
            {
                _providers.Add(entry.Key, provider);
            }
        }
    }

    private IComponentProvider? CreateProviderForPath(string key, string currentDir, FileProviderOptions? fileProviderOptions)
    {
        if (fileProviderOptions == null)
        {
            _logger.LogInformation($"no file provider options for {key}");
            return null;
        }
        
        var path = Path.Combine(currentDir, fileProviderOptions.FilePath);
        var pathExists = Directory.Exists(path);
        var baseUrl = fileProviderOptions.BaseUrl;

        if (pathExists)
        {
            _logger.LogInformation($"Adding provider for {key} with base url '{baseUrl}' to path '{path}' ");
            return _providerFactory.CreateFileProvider(key, baseUrl, path);
        }

        _logger.LogInformation($"path '{path}' doesn't exists : provider {key} not created");
        return null;
    }
}
