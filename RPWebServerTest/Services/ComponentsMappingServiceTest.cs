using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RPWebServer.Configuration;
using RPWebServer.Core;
using RPWebServer.Services;

namespace RPWebServerTest.Services;

public class ComponentsMappingServiceTest
{
    private IOptions<FileProviderOptions>[] FileProviderOptions { get; }
    private IOptions<WebComponentOptions>[] WebComponentOptions { get; }
    IOptions<WebComponentsServerOptions> WebComponentsServerOptions { get; }
    Mock<IComponentProviderFactory> ComponentProviderFactoryMock { get; }
    Mock<ILogger<ComponentsMappingService>> LoggerMock { get; }
    Mock<IComponentProvider>[] ComponentProviderMocks { get; }
    private Mock<IFileSystem> FileSystemMock { get; }

    private string[] BaseUrls { get; } = { "TestA/url", "TestB/file" };
    private string[] FilePaths { get; } = { "some/file/path", "some/other/file/path" };
    private string[] ProviderIds { get; } = {"TestA", "TestB"};
    private string[] ProviderValidUrls { get; } = {"testA/url/module/main.js", "testB/file/module/main.jsx"};
    private string[] ProviderInvalidUrls { get; } = {"some/invalid/url/main.js", "some/other/invalid/url/main.ico"};
    private string ServerRootDir { get; }= "root/folder/";

    public ComponentsMappingServiceTest()
    {
        FileProviderOptions = new []
        {
            Options.Create<FileProviderOptions>(new FileProviderOptions(
                FilePaths[0],
                BaseUrls[0] )),

            Options.Create<FileProviderOptions>(new FileProviderOptions(
                FilePaths[1],
                BaseUrls[1]
                ))
        };

        WebComponentOptions = new[]
        {
            Options.Create<WebComponentOptions>(new WebComponentOptions()
            {
                FileProvider = FileProviderOptions[0].Value
            }),
            Options.Create(new WebComponentOptions()
            {
                FileProvider = FileProviderOptions[1].Value
            })
        };

        WebComponentsServerOptions = Options.Create<WebComponentsServerOptions>(new WebComponentsServerOptions()
        {
            Root = ServerRootDir,
            WebComponents = new Dictionary<string, WebComponentOptions>()
            {
                { ProviderIds[0], WebComponentOptions[0].Value },
                { ProviderIds[1], WebComponentOptions[1].Value }
            }
        });

        ComponentProviderMocks = new[]
        {
            new Mock<IComponentProvider>(),
            new Mock<IComponentProvider>()
        };

        // setup provider 0
        {
            var providerMock = ComponentProviderMocks[0];
            providerMock
                .SetupGet(it => it.Id)
                .Returns(ProviderIds[0]);
            providerMock
                .SetupGet(it => it.BaseUrl)
                .Returns(BaseUrls[0]);
            providerMock
                .Setup(it => it.MatchUrl(ProviderInvalidUrls[0]))
                .Returns(false);
            providerMock
                .Setup(it => it.MatchUrl(ProviderValidUrls[0]))
                .Returns(true);
        }

        // setup provider 1
        {
            var providerMock = ComponentProviderMocks[1];
            providerMock
                .SetupGet(it => it.Id)
                .Returns(ProviderIds[1]);
            providerMock
                .SetupGet(it => it.BaseUrl)
                .Returns(BaseUrls[1]);
            providerMock
                .Setup(it => it.MatchUrl(ProviderInvalidUrls[1]))
                .Returns(false);
            providerMock
                .Setup(it => it.MatchUrl(ProviderValidUrls[1]))
                .Returns(true);
        }

        ComponentProviderFactoryMock = new Mock<IComponentProviderFactory>();
        
        LoggerMock = new Mock<ILogger<ComponentsMappingService>>();
        FileSystemMock = new Mock<IFileSystem>();
    }

    [Fact]
    public void ConstructorInitializeCorrectly()
    {
        var service = new ComponentsMappingService(LoggerMock.Object, ComponentProviderFactoryMock.Object, FileSystemMock.Object, WebComponentsServerOptions);

        Assert.Equal(LoggerMock.Object, service.Logger);
        Assert.Equal(ComponentProviderFactoryMock.Object, service.ProviderFactory);
        Assert.Equal(WebComponentsServerOptions, service.WebComponentsServerOptions);
    }

    [Fact]
    public void GetProviderForValidUrl()
    {
        var service = new ComponentsMappingService(LoggerMock.Object, ComponentProviderFactoryMock.Object, FileSystemMock.Object, WebComponentsServerOptions);

        service.Providers.Add(ProviderIds[0], ComponentProviderMocks[0].Object);
        service.Providers.Add(ProviderIds[1], ComponentProviderMocks[1].Object);
        
        var provider = service.GetProviderForUrl(ProviderValidUrls[0]);

        Assert.NotNull(provider);
        Assert.Equal(ProviderIds[0], provider.Id);
    }

    [Fact]
    public void GetProviderForValidUrlWithSecondProviderTaken()
    {
        var service = new ComponentsMappingService(LoggerMock.Object, ComponentProviderFactoryMock.Object, FileSystemMock.Object, WebComponentsServerOptions);

        service.Providers.Add(ProviderIds[0], ComponentProviderMocks[0].Object);
        service.Providers.Add(ProviderIds[1], ComponentProviderMocks[1].Object);
        
        var provider = service.GetProviderForUrl(ProviderValidUrls[1]);

        Assert.NotNull(provider);
        Assert.Equal(ProviderIds[1], provider.Id);
    }

    [Fact]
    public void GetProviderForInvalidUrl()
    {
        var service = new ComponentsMappingService(LoggerMock.Object, ComponentProviderFactoryMock.Object, FileSystemMock.Object, WebComponentsServerOptions);

        service.Providers.Add(ProviderIds[0], ComponentProviderMocks[0].Object);
        service.Providers.Add(ProviderIds[1], ComponentProviderMocks[1].Object);
        
        var provider = service.GetProviderForUrl(ProviderInvalidUrls[0]);

        Assert.Null(provider);
    }

    [Fact]
    public void CreateProviderForPathWithValidNonRootedPath()
    {
        var currentDirectory = "/etc/home";
        var existsDirectory = $"{currentDirectory}/{FilePaths[0]}";
        var directoryMock = new Mock<IDirectory>();
        directoryMock
            .Setup(it => it.Exists(existsDirectory))
            .Returns(true);
        var pathMock = new Mock<IPath>();
        pathMock
            .Setup(it => it.Combine(currentDirectory, FilePaths[0]))
            .Returns(existsDirectory);
        pathMock
            .Setup(it => it.IsPathRooted(FilePaths[0]))
            .Returns(false);
        
        FileSystemMock
            .SetupGet(it => it.Directory)
            .Returns(directoryMock.Object);
        FileSystemMock
            .SetupGet(it => it.Path)
            .Returns(pathMock.Object);
        
        ComponentProviderFactoryMock
            .Setup(it => it.CreateFileProvider(ProviderIds[0], BaseUrls[0], existsDirectory))
            .Returns(ComponentProviderMocks[0].Object);

        
        var service = new ComponentsMappingService(LoggerMock.Object, ComponentProviderFactoryMock.Object, FileSystemMock.Object, WebComponentsServerOptions);
        var provider = service.CreateProviderForPath(ProviderIds[0], currentDirectory, FileProviderOptions[0].Value);

        Assert.NotNull(provider);
        Assert.Equal(ProviderIds[0], provider.Id);
    }
    
    [Fact]
    public void CreateProviderForPathWithValidRootedPath()
    {
        var currentDirectory = "/etc/home";
        var directoryMock = new Mock<IDirectory>();
        directoryMock
            .Setup(it => it.Exists(FilePaths[0]))
            .Returns(true);
        var pathMock = new Mock<IPath>();
        pathMock
            .Setup(it => it.IsPathRooted(FilePaths[0]))
            .Returns(true);
        
        FileSystemMock
            .SetupGet(it => it.Directory)
            .Returns(directoryMock.Object);
        FileSystemMock
            .SetupGet(it => it.Path)
            .Returns(pathMock.Object);
        
        ComponentProviderFactoryMock
            .Setup(it => it.CreateFileProvider(ProviderIds[0], BaseUrls[0], FilePaths[0]))
            .Returns(ComponentProviderMocks[0].Object);

        
        var service = new ComponentsMappingService(LoggerMock.Object, ComponentProviderFactoryMock.Object, FileSystemMock.Object, WebComponentsServerOptions);
        var provider = service.CreateProviderForPath(ProviderIds[0], currentDirectory, FileProviderOptions[0].Value);

        Assert.NotNull(provider);
        Assert.Equal(ProviderIds[0], provider.Id);
    }
    
    [Fact]
    public void CreateProviderForPathWithNonValidRootedPath()
    {
        var currentDirectory = "/etc/home";
        var directoryMock = new Mock<IDirectory>();
        directoryMock
            .Setup(it => it.Exists(FilePaths[0]))
            .Returns(false);
        var pathMock = new Mock<IPath>();
        pathMock
            .Setup(it => it.IsPathRooted(FilePaths[0]))
            .Returns(true);
        
        FileSystemMock
            .SetupGet(it => it.Directory)
            .Returns(directoryMock.Object);
        FileSystemMock
            .SetupGet(it => it.Path)
            .Returns(pathMock.Object);
        
        ComponentProviderFactoryMock
            .Setup(it => it.CreateFileProvider(ProviderIds[0], BaseUrls[0], FilePaths[0]))
            .Returns(ComponentProviderMocks[0].Object);

        
        var service = new ComponentsMappingService(LoggerMock.Object, ComponentProviderFactoryMock.Object, FileSystemMock.Object, WebComponentsServerOptions);
        var provider = service.CreateProviderForPath(ProviderIds[0], currentDirectory, FileProviderOptions[0].Value);

        Assert.Null(provider);
    }
    
    [Fact]
    public void CreateProviderForPathWithNonValidNonRootedPath()
    {
        var currentDirectory = "/etc/home";
        var existsDirectory = $"{currentDirectory}/{FilePaths[0]}";
        var directoryMock = new Mock<IDirectory>();
        directoryMock
            .Setup(it => it.Exists(existsDirectory))
            .Returns(false);
        var pathMock = new Mock<IPath>();
        pathMock
            .Setup(it => it.Combine(currentDirectory, FilePaths[0]))
            .Returns(existsDirectory);
        pathMock
            .Setup(it => it.IsPathRooted(FilePaths[0]))
            .Returns(false);
        
        FileSystemMock
            .SetupGet(it => it.Directory)
            .Returns(directoryMock.Object);
        FileSystemMock
            .SetupGet(it => it.Path)
            .Returns(pathMock.Object);
        
        var service = new ComponentsMappingService(LoggerMock.Object, ComponentProviderFactoryMock.Object, FileSystemMock.Object, WebComponentsServerOptions);
        var provider = service.CreateProviderForPath(ProviderIds[0], currentDirectory, FileProviderOptions[0].Value);

        Assert.Null(provider);
    }
    
    [Fact]
    public void CreateProviderForPathFileProviderOptionsNull()
    {
        var currentDirectory = "/etc/home";
        
        var service = new ComponentsMappingService(LoggerMock.Object, ComponentProviderFactoryMock.Object, FileSystemMock.Object, WebComponentsServerOptions);
        var provider = service.CreateProviderForPath(ProviderIds[0], currentDirectory, null);

        Assert.Null(provider);
    }

    [Fact]
    public void UpdateMappingWebComponentsNull()
    {
        WebComponentsServerOptions.Value.WebComponents = null;

        var providerMock = ComponentProviderMocks[0].Object;
        
        var service = new ComponentsMappingService(LoggerMock.Object, ComponentProviderFactoryMock.Object, FileSystemMock.Object, WebComponentsServerOptions);
        service.Providers.Add(providerMock.Id, providerMock);

        service.UpdateMapping();

        // items shouldn't be cleared
        Assert.Single(service.Providers);
    }

    [Fact]
    public void UpdateMappingRootDirNotRooted()
    {
        var currentDir = "/etc/home";
        var rootDir = ServerRootDir;
        var combinedDir = $"{currentDir}/{rootDir}";
        var providerMock = ComponentProviderMocks[0].Object;

        var directoryMock = new Mock<IDirectory>();
        directoryMock
            .Setup(it => it.GetCurrentDirectory())
            .Returns(currentDir);
        directoryMock
            .Setup(it => it.Exists(It.IsAny<string>()))
            .Returns(true);

        var pathMock = new Mock<IPath>();
        pathMock
            .Setup(it => it.Combine(currentDir, rootDir))
            .Returns(combinedDir);
        pathMock
            .Setup(it => it.IsPathRooted(rootDir))
            .Returns(false);

        FileSystemMock
            .SetupGet(it => it.Directory)
            .Returns(directoryMock.Object);

        FileSystemMock
            .SetupGet(it => it.Path)
            .Returns(pathMock.Object);

        ComponentProviderFactoryMock
            .Setup(it => it.CreateFileProvider(ProviderIds[0], BaseUrls[0], It.IsAny<string>()))
            .Returns(ComponentProviderMocks[0].Object);
        ComponentProviderFactoryMock
            .Setup(it => it.CreateFileProvider(ProviderIds[1], BaseUrls[1], It.IsAny<string>()))
            .Returns(ComponentProviderMocks[1].Object);
        
        var service = new ComponentsMappingService(LoggerMock.Object, ComponentProviderFactoryMock.Object, FileSystemMock.Object, WebComponentsServerOptions);
        service.Providers.Add(providerMock.Id, providerMock);
        
        service.UpdateMapping();

        Assert.Equal(2, service.Providers.Count);
    }
    
    [Fact]
    public void UpdateMappingRootDirNull()
    {
        var currentDir = "/etc/home";
        var rootDir = ServerRootDir;
        var combinedDir = $"{currentDir}/{rootDir}";
        var providerMock = ComponentProviderMocks[0].Object;

        var directoryMock = new Mock<IDirectory>();
        directoryMock
            .Setup(it => it.GetCurrentDirectory())
            .Returns(currentDir);
        directoryMock
            .Setup(it => it.Exists(It.IsAny<string>()))
            .Returns(true);

        var pathMock = new Mock<IPath>();
        pathMock
            .Setup(it => it.Combine(currentDir, rootDir))
            .Returns(combinedDir);
        pathMock
            .Setup(it => it.IsPathRooted(rootDir))
            .Returns(false);

        FileSystemMock
            .SetupGet(it => it.Directory)
            .Returns(directoryMock.Object);

        FileSystemMock
            .SetupGet(it => it.Path)
            .Returns(pathMock.Object);

        ComponentProviderFactoryMock
            .Setup(it => it.CreateFileProvider(ProviderIds[0], BaseUrls[0], It.IsAny<string>()))
            .Returns(ComponentProviderMocks[0].Object);
        ComponentProviderFactoryMock
            .Setup(it => it.CreateFileProvider(ProviderIds[1], BaseUrls[1], It.IsAny<string>()))
            .Returns(ComponentProviderMocks[1].Object);

        WebComponentsServerOptions.Value.Root = null;
        
        var service = new ComponentsMappingService(LoggerMock.Object, ComponentProviderFactoryMock.Object, FileSystemMock.Object, WebComponentsServerOptions);
        service.Providers.Add(providerMock.Id, providerMock);
        
        service.UpdateMapping();

        Assert.Equal(2, service.Providers.Count);
        pathMock.Verify(it => it.Combine(currentDir, rootDir), Times.Never);
    }
}