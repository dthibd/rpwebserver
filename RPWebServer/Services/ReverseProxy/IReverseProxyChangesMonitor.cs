namespace RPWebServer.Services.ReverseProxy;

public interface IReverseProxyChangesMonitor
{
    void Update();
    
    IObservable<IReverseProxyChangesMonitor> UpdateObservable { get; }
}
