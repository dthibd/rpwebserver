using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using WebComponentServer.Configuration;
using WebComponentServer.Core;
using WebComponentServer.Services;

namespace WebComponentServerTest;

public class ComponentsMappingServiceTest
{
    private IOptions<FileProviderOptions>[] FileProviderOptions { get; }
    private IOptions<WebComponentOptions>[] WebComponentOptions { get; }
    IOptions<WebComponentsServerOptions> WebComponentsServerOptions { get; }
    Mock<IComponentProviderFactory> ComponentProviderFactoryMock { get; }
    Mock<ILogger<ComponentsMappingService>> LoggerMock { get; }
    Mock<IComponentProvider>[] ComponentProviderMocks { get; }
    private string[] BaseUrls { get; } = { "TestA/url", "TestB/file" };
    private string[] FilePaths { get; } = { "some/file/path", "some/other/file/path" };
    private string[] ProviderIds { get; } = {"TestA", "TestB"};
    private string[] ProviderValidUrls { get; } = {"testA/url/module/main.js", "testB/file/module/main.jsx"};
    private string[] ProviderInvalidUrls { get; } = {"some/invalid/url/main.js", "some/other/invalid/url/main.ico"}; 

    public ComponentsMappingServiceTest()
    {
        FileProviderOptions = new []
        {
            Options.Create<FileProviderOptions>(new FileProviderOptions()
            {
                BaseUrl = BaseUrls[0],
                FilePath = FilePaths[0]
            }),
            Options.Create<FileProviderOptions>(new FileProviderOptions()
            {
                BaseUrl = BaseUrls[1],
                FilePath = FilePaths[1]
            })

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
            Root = "root/folder/",
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
    }

    [Fact]
    public void ConstructorInitialize()
    {
        var service = new ComponentsMappingService(LoggerMock.Object, ComponentProviderFactoryMock.Object, WebComponentsServerOptions);

        Assert.Equal(LoggerMock.Object, service.Logger);
        Assert.Equal(ComponentProviderFactoryMock.Object, service.ProviderFactory);
        Assert.Equal(WebComponentsServerOptions, service.WebComponentsServerOptions);
    }

    [Fact]
    public void GetProviderForInvalidUrl()
    {
        var service = new ComponentsMappingService(LoggerMock.Object, ComponentProviderFactoryMock.Object, WebComponentsServerOptions);

        service.Providers.Add(ProviderIds[0], ComponentProviderMocks[0].Object);
        service.Providers.Add(ProviderIds[1], ComponentProviderMocks[1].Object);
        
        var provider = service.GetProviderForUrl(ProviderValidUrls[0]);

        Assert.NotNull(provider);
        Assert.Equal(ProviderIds[0], provider.Id);
    }
}