using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RPWebServer.Controllers;
using RPWebServer.Core;
using RPWebServer.Services;

namespace RPWebServerTest.Controllers;

public class ComponentsControllerTest
{
    public Mock<ILogger<ComponentsController>> LoggerMock { get; } = new();
    
    public Mock<IComponentsMappingService> ComponentsMappingServiceMock { get; } = new();

    [Fact]
    public void Construction()
    {
        var controller = new ComponentsController(
            LoggerMock.Object,
            ComponentsMappingServiceMock.Object
        );
        
        Assert.Equal(LoggerMock.Object, controller.Logger);
        Assert.Equal(ComponentsMappingServiceMock.Object, controller.ComponentsMappingService);
    }

    [Fact]
    public void IndexValidWorkflow()
    {
        var url = "test url";
        var contentResult = new ProviderContentResult()
        {
            Content = new MemoryStream(),
            ContentType = "text/json"
        };

        var providerMock = new Mock<IComponentProvider>();

        providerMock
            .Setup(it => it.GetContentForUrl(url))
            .Returns(contentResult);
        
        ComponentsMappingServiceMock
            .Setup(it => it.GetProviderForUrl(url))
            .Returns(providerMock.Object);
        
        var controller = new ComponentsController(
            LoggerMock.Object,
            ComponentsMappingServiceMock.Object
        );
        
        var result = controller.Index(url);

        Assert.NotNull(result);
        Assert.IsType<FileStreamResult>(result);

        var fileStreamResult = result as FileStreamResult;

        Assert.NotNull(fileStreamResult);
        Assert.Equal(contentResult.Content, fileStreamResult.FileStream);
        Assert.Equal(contentResult.ContentType, fileStreamResult.ContentType);
    }

    [Fact]
    public void IndexWithInvalidProvider()
    {
        var url = "test url";
        
        ComponentsMappingServiceMock
            .Setup(it => it.GetProviderForUrl(url))
            .Returns((IComponentProvider?)null);
        
        var controller = new ComponentsController(LoggerMock.Object, ComponentsMappingServiceMock.Object);
        var result = controller.Index(url);

        Assert.NotNull(result);
        Assert.IsType<BadRequestResult>(result);

        LoggerMock.Verify(
            x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)), Times.Once);
    }

    [Fact]
    public void IndexNoContent()
    {
        var url = "test url";
        var contentResult = new ProviderContentResult()
        {
            Content = null,
            ContentType = "text/json"
        };

        var providerMock = new Mock<IComponentProvider>();

        providerMock
            .Setup(it => it.GetContentForUrl(url))
            .Returns(contentResult);
        
        ComponentsMappingServiceMock
            .Setup(it => it.GetProviderForUrl(url))
            .Returns(providerMock.Object);
        
        var controller = new ComponentsController(
            LoggerMock.Object,
            ComponentsMappingServiceMock.Object
        );
        
        var result = controller.Index(url);

        Assert.NotNull(result);
        Assert.IsType<OkResult>(result);
    }
    
    [Fact]
    public void IndexWithException()
    {
        var url = "test url";
        
        ComponentsMappingServiceMock
            .Setup(it => it.GetProviderForUrl(url))
            .Throws(new Exception());
        
        var controller = new ComponentsController(LoggerMock.Object, ComponentsMappingServiceMock.Object);
        var result = controller.Index(url);

        Assert.NotNull(result);
        Assert.IsType<StatusCodeResult>(result);
    }
}