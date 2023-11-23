using WebComponentServer.Core;

namespace WebComponentServer.Services;

public interface IComponentProviderFactory
{
    IComponentProvider CreateFileProvider(string id, string baseUrl, string sourcePath);
}
