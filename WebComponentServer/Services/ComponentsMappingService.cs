using Microsoft.Extensions.Options;
using WebComponentServer.Configuration;
using WebComponentServer.Core;

namespace WebComponentServer.Services;


public class ComponentsMappingService : IComponentsMappingService
{
    private ILogger<ComponentsMappingService> _logger;
    private IOptions<WebComponentsServerOptions> _webComponentsServerOptions;

    private Dictionary<string, IComponentProvider> _providers = new Dictionary<string, IComponentProvider>();

    public ComponentsMappingService(
        ILogger<ComponentsMappingService> logger,
        IOptions<WebComponentsServerOptions> options)
    {
        _logger = logger;
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
        if (webComponents != null)
        {
            var currentDir = Directory.GetCurrentDirectory();
            var rootDir = _webComponentsServerOptions.Value?.Root;

            if (!string.IsNullOrEmpty(rootDir))
            {
                currentDir = Path.Combine(currentDir, rootDir);
            }
            
            foreach (var entry in webComponents)
            {
                var fileProviderOptions = entry.Value?.FileProvider;
                if (fileProviderOptions != null)
                {
                    var path = Path.Combine(currentDir, fileProviderOptions.FilePath);
                    var pathExists = Directory.Exists(path);
                    var baseUrl = fileProviderOptions.BaseUrl;

                    if (pathExists)
                    {
                        _logger.LogInformation($"Adding provider for {entry.Key} with base url '{baseUrl}' to path '{path}' ");

                        var provider = new ComponentFileProvider(entry.Key, baseUrl, path);
                        _providers.Add(entry.Key, provider);
                    }
                    else
                    {
                        _logger.LogInformation($"path '{path}' doesn't exists : provider {entry.Key} not created");
                    }
                }
            }
        }
    }
}
