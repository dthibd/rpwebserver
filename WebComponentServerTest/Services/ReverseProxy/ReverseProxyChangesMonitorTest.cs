using Moq;
using WebComponentServer.Services.ReverseProxy;

namespace WebComponentServerTest.Services.ReverseProxy;

public class MonitorListener
{
    public virtual void OnTriggered(IReverseProxyChangesMonitor monitor)
    {
        
    }
}

public class ReverseProxyChangesMonitorTest
{
    [Fact]
    public void UpdateCallsObservers()
    {
        var monitor = new ReverseProxyChangesMonitor();
        var listenerMock = new Mock<MonitorListener>();
        listenerMock
            .Setup(it => it.OnTriggered(It.IsAny<IReverseProxyChangesMonitor>()));

        monitor.UpdateObservable
            .Subscribe(listenerMock.Object.OnTriggered);

        monitor.Update();
        
        listenerMock.Verify(it => it.OnTriggered(It.IsAny<IReverseProxyChangesMonitor>()), Times.Once);
    }
}