using System.IO.Abstractions;
using WebComponentServer.Core;

namespace WebComponentServer.Services;

public class ComponentProviderFactory : IComponentProviderFactory
{
    private IFileSystem _fileSystem;

    public ComponentProviderFactory(
        IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public IComponentProvider CreateFileProvider(string id, string baseUrl, string sourcePath)
    {
        return new ComponentFileProvider(_fileSystem, id, baseUrl, sourcePath);
    }
}