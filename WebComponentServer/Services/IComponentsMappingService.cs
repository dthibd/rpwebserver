using WebComponentServer.Core;

namespace WebComponentServer.Services;

public interface IComponentsMappingService
{
    IComponentProvider? GetProviderForUrl(string url);
}