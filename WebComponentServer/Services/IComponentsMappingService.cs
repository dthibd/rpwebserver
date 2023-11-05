using WebComponentServer.Core;

namespace WebComponentServer.Services;

public interface IComponentsMappingService
{
    void UpdateMapping();
    IComponentProvider? GetProviderForUrl(string url);
}