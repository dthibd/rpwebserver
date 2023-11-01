using Microsoft.Extensions.Options;
using WebComponentServer.Configuration;

namespace WebComponentServer.Services;

public class ComponentsMappingService : IComponentsMappingService
{
    private ILogger<ComponentsMappingService> _logger;
    private IOptions<WebComponentsServerOptions> _webComponentsServerOptions;

    public ComponentsMappingService(
        ILogger<ComponentsMappingService> logger,
        IOptions<WebComponentsServerOptions> options)
    {
        _logger = logger;
        _webComponentsServerOptions = options;

        CreateMapping();
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
                    _logger.LogDebug($"{entry.Key} -- {path} -- exists : {pathExists}");
                }
            }
        }
    }
}