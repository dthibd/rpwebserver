using Moq;
using RPWebServer.Services.ReverseProxy;

namespace RPWebServerTest.Services.ReverseProxy;

public class ReverseProxyChangesMonitorTest
{
    [Fact]
    public void UpdateCallsObservers()
    {
        var monitor = new ReverseProxyChangesMonitor();
        var observerMock = new Mock<IObserver<IReverseProxyChangesMonitor>>();
        

        monitor.UpdateObservable
            .Subscribe(observerMock.Object);
        
        monitor.Update();
        
        observerMock.Verify(it => it.OnNext(monitor), Times.Once);
    }
}