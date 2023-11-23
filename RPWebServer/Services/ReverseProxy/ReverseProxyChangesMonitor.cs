using System.Reactive.Subjects;

namespace WebComponentServer.Services.ReverseProxy;

public class ReverseProxyChangesMonitor : IReverseProxyChangesMonitor
{
    private readonly Subject<IReverseProxyChangesMonitor> _updateSubject = new Subject<IReverseProxyChangesMonitor>();

    public void Update()
    {
        _updateSubject.OnNext(this);
    }

    public IObservable<IReverseProxyChangesMonitor> UpdateObservable => _updateSubject;
}

