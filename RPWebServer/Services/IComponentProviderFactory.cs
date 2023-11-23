using RPWebServer.Core;

namespace RPWebServer.Services;

public interface IComponentProviderFactory
{
    IComponentProvider CreateFileProvider(string id, string baseUrl, string sourcePath);
}
