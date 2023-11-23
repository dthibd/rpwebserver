using RPWebServer.Core;

namespace RPWebServer.Services;

public interface IComponentsMappingService
{
    void UpdateMapping();
    IComponentProvider? GetProviderForUrl(string url);
}