using Moq;
using RPWebServer.Commands.ReverseProxy;
using RPWebServer.Handlers.ReverseProxy;
using RPWebServer.Services.ReverseProxy;

namespace RPWebServerTest.Handlers.ReverseProxy;

public class RefreshReverseProxyHandlerTest
{
    [Fact]
    public async void HandlerCallsUpdate()
    {
        var monitorMock = new Mock<IReverseProxyChangesMonitor>();
        monitorMock
            .Setup(it => it.Update());

        var handler = new RefreshReverseProxyHandler(monitorMock.Object);
        await handler.Handle(new RefreshReverseProxyRequest(), new CancellationToken());

        monitorMock.Verify(it => it.Update(), Times.Once);
    }
}